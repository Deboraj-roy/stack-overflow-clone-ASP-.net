# DEBORAJ-ROY-B9
Full Stack Asp.net Core MVC Web Development Batch - 9
By - Jalal Uddin

## Befour you run the Application check Connection Strings and Environment variables
- set web.env File in same location of Program.cs

- but in run application
``DefaultConnection="Server=192.168.0.2,40002\\SQLEXPRESS\\SQLEXPRESS;Database=Database;User Id=User name;Password=Password;Trust Server Certificate=True;"``
- For Docker

``ConnectionStrings:DefaultConnection="Server=IP,Port\\SQLEXPRESS;Database=Database;User Id=User name;Password=Password;Trust Server Certificate=True;"
API_URL=https://api.example.com
AWS_ACCESS_KEY_ID=your_access_key_id
AWS_SECRET_ACCESS_KEY=your_secret_access_key``

## Functional requirements Applied 
- Users can register and login. Email verification
- resend Email verifications, confirmation.
- Image validation server side and client side
- Add Jwt Authentication Configuration
- Bootswatch Theme and Bootstrap Icons
- Toastr Notification
- SweetAlerts
- API to Web APP Communication 
- Mailkit, mailtrap mailconfigure and send mail
- Rich text area added, Users can use markdown editors to post code in questions and replies
- Image validatetion serversite and client side

## Non functional requirements Applied 
- Dockerize the application to make it easily deployable.
- Use clean architecture to maintain flexible design.
- ReCaptcha implemented
- Use AWS bucket
- Integrate logger and use exception handling for fault tolerance.
- Use autofac and automapper to make the code flexible.
- Use entity framework, repository and unit of work pattern to make the project robust. 
- All migrations should be added including seed data to make the project maintainable.
- Use claim based authentication and authorization to control user access to the features.
- All menu links have to be correct. Users should be able to use and navigate all features. 