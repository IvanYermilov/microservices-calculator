# microservices-calculator

# App Vision
A web application implementing the functionality of a calculator. The application does not involve a front-end part. Interaction is carried out through Postman.

## Logic Requirements
- The application should implement addition operation.
- The application should implement subtraction operation.
- The application should implement multiplication operation.
- The application should implement division operation (division by 0 is an error).
- The application should perform expression calculation (without mathematical brackets) for up to 9 operations.
- The application should prioritize operations according to mathematical rules.
- The application should record the calculation result and the expression itself in the database.
- Intermediate calculations MUST be recorded in the database as well as the final result.
- The application should return the calculation result via HTTP.
- The application should work with rational numbers (overflow - numbers during expression calculation do not fit into the double range - is an error).
- The application should provide accuracy up to the 3rd decimal place (at each operation).
- The application should round all input fractional numbers to the 3rd decimal place.
- The application should validate input data: only mathematical operations that the application can handle and numbers are allowed (the symbol "." (dot) serves as a separator).
- Authentication and authorization must be implemented.

## Architecture Requirements
- The application should be implemented based on microservices architecture.
- Each mathematical operation should be performed in a separate microservice.
- A separate microservice should handle the incoming request.
- A separate microservice should handle the recording of the expression with the result.
- Use a message broker for communication between services.
- Use the CQRS pattern for interaction between services and the database.
  - Command: for recording the result of operations in the database.
  - Query: for obtaining the result of intermediate operations.
- Database: MongoDB.
- Implement the SAGA pattern in the application.

## Communication Channel Requirements
- Message broker: RabbitMQ.

## Scaling and Deployment Requirements
- Microservices, the database, and the message broker should be deployed in Kubernetes.
- Horizontal scaling of services should be implemented.
