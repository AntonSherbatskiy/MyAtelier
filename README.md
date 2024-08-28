# MyAtelier
___
This web application was created to manage an atelier outlet. Users can
order clothing repair and tailoring services by choosing one of the
categories.Users can also leave additional information to the order. All the
data provided during registration is stored in the personal account.
Managers keep track of the warehouse, services, clothes, and orders, with
the ability to add and remove services. The administrator, can manage
user accounts, add new ones, delete inactive accounts, edit information,
and view order history.

### Features:

* Electronic customer account
* Online ordering of tailoring or clothing repair services
* User account management
* Email sending service
* Material inventory management
* Management of tailoring and clothing repair orders
* Clothing inventory for available services
* Service history viewing
* User order management
* Atelier service management

## Installation
### Prerequisites
* Docker

Clone the repository
```bash
cd myatelier
git clone https://github.com/AntonSherbatskiy/MyAtelier.git
```
This application uses email sending, so it is necessary to have an email address to send emails, so you need to specify some parameters

Open `docker-compose.yml`

Copy your `Mailer Email` to `EmailSenderOptions__Email`

Copy your `Mailer Password` to `EmailSenderOptions__Password`

If you want to use your own database, additionally insert your connection string into `ConnectionStrings__DefaultConnectionString`.

Run docker compose
```bash
docker compose up -d
```

Now you have 2 containers in the docker: `backend`, `web-site`

To get to the main page, open your browser and go to http://localhost