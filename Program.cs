using System.ClientModel;
using System.Reflection;
using System.Text.RegularExpressions;
using AutoGen.Core;
using AutoGen.OpenAI;
using AutoGen.OpenAI.Extension;
using Azure.AI.OpenAI;
using OpenAI;

// Initialize OpenAI client
var endpoint = "http://localhost:1234";
var openaiClient = new OpenAI.OpenAIClient(new ApiKeyCredential("api-key"), new OpenAI.OpenAIClientOptions
{
    Endpoint = new Uri(endpoint),
});

// Initialize chat agent
var lmAgent = new OpenAIChatAgent(
    chatClient: openaiClient.GetChatClient("<does-not-matter>"),
    name: "assistant",
    systemMessage: @"You are an SQL Server expert that generates optimized T-SQL queries for a matriculation examination system.

SCHEMA OVERVIEW:
Students (MATRIC_ADMISSION) -> Centers (CENTER) -> Labs (PRACTICAL_LABS) -> Schedule (PRACTICAL_DATESHEET)

KEY TABLES AND CONSTRAINTS:
1. MATRIC_ADMISSION
   PK: FORMNO int [clustered]  
   COLUMNS: RNO [unique], CENTRE/ZONE_CODE [FK]
   RULES: SEX IN ('F','M'), STATUS IN ('P','R')
   INDEXES: IX_RNO, IX_CENTRE_ZONE

2. CENTER
   PK: ID int, CENTRE int [unique]
   MAIN: CAPACITY int, GENDER int
   INDEX: IX_ZONE(ZONE_CODE) INCLUDE(CAPACITY)

3. PRACTICAL_LABS
   PK: LAB_CODE int
   CORE: SUBJECT IN ('BIO','PHY','CHE')
   LIMITS: CAPACITY, SHIFT_CAPACITY

4. PRACTICAL_DATESHEET
   KEY: SUBJECT [FK], EXAM_DATE, SHIFT
   CHECK: MAX_STUDENTS <= LAB.SHIFT_CAPACITY

VALIDATION REQUIREMENTS:
1. Demographics: Always validate SEX, STATUS, S_GROUP, RELIGION
2. Capacity: Check center and lab limits
3. Relationships: Enforce CENTRE+ZONE pairs

PATTERN EXAMPLE:
IF NOT EXISTS (
    SELECT 1 FROM CENTER 
    WHERE CENTRE = @Centre
    AND ZONE_CODE = @Zone
    AND CAPACITY > (
        SELECT COUNT(*) 
        FROM MATRIC_ADMISSION
        WHERE CENTRE = @Centre
    )
)
    RAISERROR('Invalid center or capacity', 16, 1);

RETURN ONLY SQL WITH:
1. Required validations
2. Proper JOIN conditions
3. Error handling
4. Clear comments")
    .RegisterMessageConnector();

// Maintain message history
var messageHistory = new List<IMessage>
{
    // Add a system message to define the assistant's behavior
    new TextMessage(Role.System, System.IO.File.ReadAllText("system_message.txt"))
};

string? UserQuery;