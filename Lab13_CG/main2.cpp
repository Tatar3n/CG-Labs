#include <SFML/Graphics.hpp>
#include <GL/glew.h>

#include <algorithm>
#include <iostream>
#include <vector>
#include <cmath>

#include "shader.h"
#include "camera.h"
#include "model.h"

const unsigned int SCR_WIDTH = 800;
const unsigned int SCR_HEIGHT = 600;

Camera camera(glm::vec3(0.0f, 50.0f, 200.0f));
float lastX = (float)SCR_WIDTH / 2.0;
float lastY = (float)SCR_HEIGHT / 2.0;
bool firstMouse = true;


float deltaTime = 0.0f;
float lastFrame = 0.0f;


void Init()
{

    glEnable(GL_DEPTH_TEST); // глубина, чтоб 3д объекты норм отображались
}


void processInput(sf::Window& window)
{
    if (sf::Keyboard::isKeyPressed(sf::Keyboard::Key::Escape))
        window.close();

    if (sf::Keyboard::isKeyPressed(sf::Keyboard::Key::W))
        camera.ProcessKeyboard(FORWARD, deltaTime);
    if (sf::Keyboard::isKeyPressed(sf::Keyboard::Key::S))
        camera.ProcessKeyboard(BACKWARD, deltaTime);
    if (sf::Keyboard::isKeyPressed(sf::Keyboard::Key::A))
        camera.ProcessKeyboard(LEFT, deltaTime);
    if (sf::Keyboard::isKeyPressed(sf::Keyboard::Key::D))
        camera.ProcessKeyboard(RIGHT, deltaTime);
    if (sf::Keyboard::isKeyPressed(sf::Keyboard::Key::Up))
        camera.ProcessKeyboard(UP, deltaTime);
    if (sf::Keyboard::isKeyPressed(sf::Keyboard::Key::Down))
        camera.ProcessKeyboard(DOWN, deltaTime);
    if (sf::Keyboard::isKeyPressed(sf::Keyboard::Key::Left))
        camera.ProcessKeyboard(ROTATE_LEFT, deltaTime);
    if (sf::Keyboard::isKeyPressed(sf::Keyboard::Key::Right))
        camera.ProcessKeyboard(ROTATE_RIGHT, deltaTime);



    sf::Vector2i mousePosition = sf::Mouse::getPosition(window);
    float xpos = static_cast<float>(mousePosition.x);
    float ypos = static_cast<float>(mousePosition.y);

    if (firstMouse)
    {
        lastX = xpos;
        lastY = ypos;
        firstMouse = false;
    }

    float xoffset = xpos - lastX;
    float yoffset = lastY - ypos;  
    lastX = xpos;
    lastY = ypos;

    camera.ProcessMouseMovement(xoffset, yoffset);
}

int main()
{
    setlocale(LC_ALL, "ru");

    sf::RenderWindow window(sf::VideoMode({ 800, 600 }), "SFML window");

    window.setVerticalSyncEnabled(true);

    window.setMouseCursorVisible(false);

    glewInit();

    Init();

    Shader asteroidShader("10.3.asteroids.vs", "10.3.asteroids.fs");
    Shader planetShader("10.3.planet.vs", "10.3.planet.fs");

    
    Model planet("resources/tree/tree.obj");
    Model rock("resources/snow/model.obj");

    
    unsigned int amount = 10;
    glm::mat4* modelMatrices;
    modelMatrices = new glm::mat4[amount];
    srand(static_cast<unsigned int>(time(NULL))); 
    float radius = 25.0f;
    float offset = 10.0f;

    std::vector<glm::vec3> initialPositions(amount);

    for (unsigned int i = 0; i < amount; i++)
    {
        glm::mat4 model = glm::mat4(1.0f);
        float angle = (float)i / (float)amount * 360.0f;
        float displacement = (rand() % (int)(2 * offset * 100)) / 100.0f - offset;
        float x = sin(angle) * radius + displacement;
        displacement = (rand() % (int)(2 * offset * 100)) / 100.0f - offset;
        float y = displacement * 0.4f; 
        displacement = (rand() % (int)(2 * offset * 100)) / 100.0f - offset;
        float z = cos(angle) * radius + displacement;
        model = glm::translate(model, glm::vec3(x, y, z));

        float scale = static_cast<float>((rand() % 20) / 100.0 + 0.05);
        model = glm::scale(model, glm::vec3(scale));

        float rotAngle = static_cast<float>((rand() % 360));
        model = glm::rotate(model, rotAngle, glm::vec3(0.4f, 0.6f, 0.8f));
        initialPositions[i] = glm::vec3(x, y, z);
        modelMatrices[i] = model;
    }


    unsigned int buffer;
    glGenBuffers(1, &buffer);
    glBindBuffer(GL_ARRAY_BUFFER, buffer);
    glBufferData(GL_ARRAY_BUFFER, amount * sizeof(glm::mat4), &modelMatrices[0], GL_STREAM_DRAW);

    
    for (unsigned int i = 0; i < rock.meshes.size(); i++)
    {
        unsigned int VAO = rock.meshes[i].VAO;
        glBindVertexArray(VAO);
        glEnableVertexAttribArray(3);
        glVertexAttribPointer(3, 4, GL_FLOAT, GL_FALSE, sizeof(glm::mat4), (void*)0);
        glEnableVertexAttribArray(4);
        glVertexAttribPointer(4, 4, GL_FLOAT, GL_FALSE, sizeof(glm::mat4), (void*)(sizeof(glm::vec4)));
        glEnableVertexAttribArray(5);
        glVertexAttribPointer(5, 4, GL_FLOAT, GL_FALSE, sizeof(glm::mat4), (void*)(2 * sizeof(glm::vec4)));
        glEnableVertexAttribArray(6);
        glVertexAttribPointer(6, 4, GL_FLOAT, GL_FALSE, sizeof(glm::mat4), (void*)(3 * sizeof(glm::vec4)));

        glVertexAttribDivisor(3, 1);
        glVertexAttribDivisor(4, 1);
        glVertexAttribDivisor(5, 1);
        glVertexAttribDivisor(6, 1);

        glBindVertexArray(0);
    }


    sf::Clock clock;


    std::vector<glm::vec3> rotationAxes(amount);
    std::vector<float> rotationSpeeds(amount);
    for (unsigned int i = 0; i < amount; i++) {
        rotationAxes[i] = glm::normalize(glm::vec3(
            static_cast<float>(rand() % 100) / 100.0f,
            static_cast<float>(rand() % 100) / 100.0f,
            static_cast<float>(rand() % 100) / 100.0f
        ));

        rotationSpeeds[i] = static_cast<float>(rand() % 100) / 5.0f;
    }
    std::vector<float> orbitAngles(amount, 0.0f);
    std::vector<float> selfRotationAngles(amount, 0.0f);

    while (window.isOpen())
    {
        float currentFrame = clock.getElapsedTime().asSeconds();
        deltaTime = (currentFrame - lastFrame)*10;
        lastFrame = currentFrame;

        processInput(window);

        const float orbitSpeed = 0.5f; 
        for (unsigned int i = 0; i < amount; i++)
        {
            glm::mat4 model = glm::mat4(1.0f);

            orbitAngles[i] += orbitSpeed * deltaTime; 
            if (orbitAngles[i] > 360.0f)
                orbitAngles[i] -= 360.0f; 
            float orbitAngleRad = glm::radians(orbitAngles[i]);

            glm::mat4 orbitRotation = glm::rotate(glm::mat4(1.0f), orbitAngleRad, glm::vec3(0.0f, 1.0f, 0.0f));
            glm::vec3 position = initialPositions[i]; 
            glm::vec4 newPosition = orbitRotation * glm::vec4(position, 1.0f);

            model = glm::translate(model, glm::vec3(newPosition));

            selfRotationAngles[i] += rotationSpeeds[i] * deltaTime;
            if (selfRotationAngles[i] > 360.0f) {
                selfRotationAngles[i] -= 360.0f; 
            }
            float selfRotationAngleRad = glm::radians(selfRotationAngles[i]);

            model = glm::rotate(model, selfRotationAngleRad, rotationAxes[i]);
            model = glm::scale(model, glm::vec3(2.0f, 2.0f, 2.0f));
            modelMatrices[i] = model;
        }



        glBindBuffer(GL_ARRAY_BUFFER, buffer);
        glBufferSubData(GL_ARRAY_BUFFER, 0, amount * sizeof(glm::mat4), &modelMatrices[0]);

        glClearColor(0.1f, 0.1f, 0.1f, 1.0f);
        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);



        glm::mat4 projection = glm::perspective(glm::radians(45.0f), (float)800 / (float)600, 0.1f, 1000.0f);
        glm::mat4 view = camera.GetViewMatrix(); 
        asteroidShader.use();
        asteroidShader.setMat4("projection", projection);
        asteroidShader.setMat4("view", view);
        planetShader.use();
        planetShader.setMat4("projection", projection);
        planetShader.setMat4("view", view);

        glm::mat4 model = glm::mat4(1.0f);
        model = glm::translate(model, glm::vec3(0.0f, -3.0f, 0.0f));
        model = glm::scale(model, glm::vec3(100.0f, 100.0f,100.0f));
        
        planetShader.setMat4("model", model);
        planet.Draw(planetShader);

        asteroidShader.use();
        asteroidShader.setInt("texture_diffuse1", 0);
        glActiveTexture(GL_TEXTURE0);
        glBindTexture(GL_TEXTURE_2D, rock.textures_loaded[0].id);
       /* for (unsigned int i = 0; i < rock.meshes.size(); i++) {
           
        }*/
        glBindVertexArray(rock.meshes[0].VAO);
        glDrawElementsInstanced(GL_TRIANGLES, static_cast<unsigned int>(rock.meshes[0].indices.size()), GL_UNSIGNED_INT, 0, amount);
        glBindVertexArray(0);
        
       
        window.display();

        while (const std::optional event = window.pollEvent())
        {
            if (event->is<sf::Event::Closed>())
                
                window.close();
        }
    }
    
    return 0;
}
