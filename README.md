# 🏝️ TravelBooking Platform

![.NET 9](https://img.shields.io/badge/.NET%209-512BD4)
![C#](https://img.shields.io/badge/C%23-239120)
![Entity Framework Core](https://img.shields.io/badge/EF%20Core-512BD4)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927)
![Swagger](https://img.shields.io/badge/Swagger-85EA2D)
![Stripe](https://img.shields.io/badge/Stripe-635BFF)
![SignalR](https://img.shields.io/badge/SignalR-512BD4)



A modern, full-stack travel booking platform. TravelBooking allows users to browse and book tours, hotels, flights, and cars, while travel agencies can manage their services via an intuitive admin dashboard. The platform is built for performance, scalability, and a seamless user experience.

🌍 **Live Demo** → [Pyramigo](http://pyramigo.duckdns.org/home)

---

## ✨ Features

The **TravelBooking API** provides a full-featured travel booking platform:

### 👥 User Management
- User registration and login with **JWT authentication**.  
- Role-based access control: **User, Admin (per service), SuperAdmin**.  
- Manage user profiles and account settings.  

### 💳 Payments
- **Stripe integration** for secure online payments.  
- Manage payment history and handle refunds.  

### 🏖️ Tours
- Browse and filter tours by **destination, price, and date**.  
- View detailed tour info including **images, itinerary, and tickets**.  
- Users can **review tours** and add them to their **wishlist**.  
- **Tour Admin** can manage tour listings and tickets.  

### 🏨 Hotels
- Search hotels by **location, category, and availability**.  
- View **room types, prices, and images**.  
- Book rooms securely.  
- Users can **review hotels** and add them to their **wishlist**.  
- **Hotel Admin** can manage hotels and rooms.  

### 🚗 Car Rentals
- Browse cars by **company, type, and price**.  
- Rent cars with booking management.  
- Users can add cars to their **wishlist**.  
- **Car Rental Admin** can manage car listings and rental companies.  

### ✈️ Flights
- Search flights by **departure, destination, date, and airline**.  
- Book flights with passenger details.  
- Users can add flights to their **wishlist**.  
- **Flight Admin** can manage flights and airlines.  

### 📝 Reviews & Wishlist
- Users can **add reviews and ratings** for tours, hotels, cars, and flights.  
- Wishlist functionality for all services for easy future booking.  

### 🏢 Admin Dashboard
- **Service-specific admins** can manage tours, hotels, car rentals, and flights.  
- **Site-wide SuperAdmin** manages all entities, users, and roles.  
- View analytics and reports for bookings, revenue, and reviews.


## 📗 Table of Contents
- [📖 About the Project](#-about-the-project)
- [🛠 Built With](#-built-with)
  - [Tech Stack](#tech-stack)
- [🚀 Live Demo](#-live-demo)
- [💻 Getting Started](#-getting-started)
  - [Prerequisites](#prerequisites)
  - [Setup](#setup)
  - [Install](#install)
  - [Usage](#usage)
  - [Deployment](#deployment)
- [👥 Authors](#-authors)
- [🔭 Future Features](#-future-features)
- [🤝 Contributing](#-contributing)
- [⭐️ Show your support](#️-show-your-support)
- [🙏 Acknowledgements](#-acknowledgements)
- [❓ FAQ](#-faq)
- [📝 License](#-license)

---

## 📖 About the Project

**TravelBooking** is a modern booking platform where users can:  
- Browse tours, hotels, cars, and flights.  
- Make secure bookings and payments.  
- Agencies can manage services through an **Admin Dashboard**.  

Repositories:  
- **Backend (ASP.NET Core API)** → [TravelBooking](https://github.com/Ahmedabdelfatah11/TravelBooking)  
- **Admin Dashboard (Angular)** → [Travel-Booking-Admin-Dashboard](https://github.com/Ahmedabdelfatah11/Travel-Booking-Admin-Dashboard)  
- **User Website (Angular)** → [TravelBookingAngularProject](https://github.com/Ahmedabdelfatah11/TravelBookingAngularProject)

---

## 🛠 Built With

### Tech Stack
- **Backend**: ASP.NET Core 9, Entity Framework Core, **SQL Server**  
- **Frontend**: Angular 20, RxJS, Bootstrab 
- **Infrastructure**: Stripe  

---

## 🚀 Live Demo
🌍 [Pyramigo - Live Site](http://pyramigo.duckdns.org/home)

---

## 💻 Getting Started

### Prerequisites
- .NET 9 SDK  
- Node.js & Angular CLI  
- **SQL Server** (local or remote)  

### Setup
Clone all repositories:
```bash
git clone https://github.com/Ahmedabdelfatah11/TravelBooking.git
git clone https://github.com/Ahmedabdelfatah11/Travel-Booking-Admin-Dashboard.git
git clone https://github.com/Ahmedabdelfatah11/TravelBookingAngularProject.git
```

## Install

Restore dependencies:

```bash
cd TravelBooking
dotnet restore
```

## Usage

Run the backend:
```
dotnet run
```

## Run Angular frontend apps:
```
cd Travel-Booking-Admin-Dashboard
npm install
ng serve

cd TravelBookingAngularProject
npm install
ng serve
```

## Deployment

Backend can be deployed on Azure / VPS with SQL Server

Angular apps can be hosted on Netlify, Vercel, or served via Nginx

## 👥 Authors

- [Ahmed Abdelfatah](https://github.com/Ahmedabdelfatah11)  
- [Mohamed Sayed](https://github.com/mohamed200184)  
- [Abanoub Emad](https://github.com/Abanoubemad21)  
- [Ayman Abdelnaby](https://github.com/AymanAbdelnaby12)  
- [Ahmed Elmahdy](https://github.com/ahmedelmahdy77)


## 🔭 Future Features

 Multi-language support 🌐

 AI-powered travel recommendations 🤖

 Mobile app integration 📱

## ⭐️ Show your support

If you like this project, please ⭐ it on GitHub!

## 🙏 Acknowledgements

Inspired by Booking.com & Expedia

Thanks to the open-source community ❤️

## ❓ FAQ

Q: Can I use this project for my agency?
A: Yes, it’s open-source under MIT License.

## 📝 License

This project is MIT
 licensed. 
