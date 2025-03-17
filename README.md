# SQL Query Assistant with LM Studio Server Integration Locally

This C# application provides an intelligent SQL Server assistant powered by OpenAI's language models. It helps users generate and validate SQL queries through natural language interactions, leveraging OpenAI's capabilities to ensure accurate and efficient SQL script generation.

The application serves as a bridge between users and SQL Server, offering AI-powered assistance for SQL query writing. It integrates with OpenAI's API to understand natural language requests and convert them into proper SQL syntax, making database interactions more accessible and reducing common SQL syntax errors.

## Repository Structure
```
.
├── Program.cs          # Main application entry point containing OpenAI client and chat agent initialization
└── .gitignore         # Git configuration file specifying which files to exclude from version control
```

## Usage Instructions
### Prerequisites
- .NET 6.0 SDK or later
- An OpenAI API key
- Visual Studio 2019/2022 or VS Code with C# extensions
- Internet connectivity for OpenAI API access
- LLM model (download from [LM Studio](https://lmstudio.ai/models) or [Hugging Face](https://huggingface.co/models))

### Installation

1. Clone the repository:
```bash
git clone <repository-url>
```

2. Navigate to the project directory:
```bash
cd <project-directory>
```

3. Restore dependencies:
```bash
dotnet restore
```

### Quick Start

1. Configure your OpenAI API key in Program.cs:
```csharp
var openaiClient = new OpenAI.OpenAIClient(new ApiKeyCredential("your-api-key-here"), new OpenAI.OpenAIClientOptions
{
    Endpoint = new Uri("your-endpoint-here"),
});
```

2. Build and run the application:
```bash
dotnet build
dotnet run
```

### More Detailed Examples
The application is configured with a SQL Server-specific system message that helps generate SQL queries. Example usage:

```csharp
// The chat agent is initialized with SQL Server expertise
var lmAgent = new OpenAIChatAgent(
    chatClient: openaiClient.GetChatClient("<model-name>"),
    name: "assistant",
    systemMessage: "You are an SQL Server assistant..."
);
```

### Troubleshooting

Common issues and solutions:

1. API Connection Issues
   - Error: "Unable to connect to OpenAI API"
   - Solution: Verify your API key and endpoint URL
   - Check your internet connection

2. Authentication Errors
   - Error: "Invalid API key provided"
   - Solution: Ensure your API key is correctly set in the configuration
   - Verify API key permissions and quota

3. Runtime Errors
   - Error: "System.Net.Http.HttpRequestException"
   - Solution: Check endpoint availability
   - Verify firewall settings

## Data Flow
The application processes user inputs through OpenAI's language models to generate SQL queries.

```ascii
[User Input] -> [OpenAI Client] -> [Chat Agent] -> [SQL Query Output]
     ^                                                    |
     |                                                    |
     +----------------------------------------------------
```

Key component interactions:
- User provides natural language query requests
- OpenAI client authenticates and manages API communication
- Chat agent processes the input with SQL-specific context
- System returns formatted SQL queries
- Error handling occurs at each step with appropriate feedback