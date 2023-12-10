# Test TSA Server "Posta Rs"

This project contains tests (example) for the TSA Server "Posta Rs". It uses Docker to ensure a consistent testing environment.

## Prerequisites

Before you begin, ensure you have met the following requirements:

* You have installed the latest version of [Docker](https://www.docker.com/products/docker-desktop).
* Note: The necessary root CA certificate can be found in the Dockerfile of this project.
* Test TSA (Time-Stamping Authority) https://test-tsa.ca.posta.rs/index.html

## Running the Tests

To run the tests, follow these steps:

1. Clone this repository to your local machine.
2. Navigate to the project folder using the command: `cd TestTSAPostaRs`.
3. Build the Docker image command: `docker build --pull --rm -f "TestTSAPostaRs/Dockerfile" -t test-tsa-posta-rs:latest "."`
4. Run the tests with the following: `docker run --name tsa-test-container test-tsa-posta-rs:latest`
    #### Expected Output:
    ```
    Timestamp token received and verified successfully.
    Timestamp token received and verified successfully.
    Timestamp token received and verified successfully.
    ```

5. Remove container with the following: `docker rm tsa-test-container` 
6. Remove image with the following: `docker rmi test-tsa-posta-rs:latest`

## Contact:
If you want to contact me you can reach me at smikaric@gmail.com.