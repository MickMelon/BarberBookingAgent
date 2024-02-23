# Barber Booking Agent

## Introduction

The Barber Booking Agent is an R&D project to investigate and explore Semantic Kernel and Large Language Models (LLMs), in this case OpenAI GPT models.

It provides a chat-based functionality to allow a user to book an appointment with a barber.

## Getting Started

Clone the repository:

```bash
git clone https://github.com/mickmelon/barberbookingagent
```

### Configuring the Web API

Set your OpenAI credentials as dotnet user secrets:

```bash
cd backend/BarberBookingAgent.WebApi
dotnet user-secrets set "OpenAiOptions:ApiKey" "{your openai api key}"
dotnet user-secrets set "OpenAiOptions:OrganizationId" "{your openai organization id}"
```

You can configure the OpenAI model by going to `appsettings.Development.json` in the `BarberBookingAgent.WebApi` project.

Start the API via your IDE or command:

```bash
dotnet run
```

## Setting up the React.js Frontend

Navigate to the frontend application directory:

```bash
cd frontend
```

Install dependencies:

```bash
npm install
```

Start the application:

```bash
npm start
```

After completing these steps, the React.js frontend application will be running and connected to your Web API, ready to handle chat interactions and booking appointments.
