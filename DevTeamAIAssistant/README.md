
# DevTeam AI Assistant

> An AI-powered toolkit for software development managers built with .NET 8 and Claude AI

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![C#](https://img.shields.io/badge/C%23-12.0-239120)](https://docs.microsoft.com/en-us/dotnet/csharp/)

**Demonstrates:** AI Integration • Prompt Engineering • .NET Architecture • Management Tools

---

## 📖 Overview

DevTeam AI Assistant helps development managers make data-driven decisions by leveraging Large Language Models (LLMs) for analysis, review, and prioritization tasks.

**Built by:** Jacques Steward - Principle Consultant
**Tech Stack:** C# • .NET 8 • Anthropic Claude API • System.Text.Json  
**Purpose:** Demonstrates practical LLM integration in enterprise .NET applications

---
## 🧪 Testing

This project includes comprehensive unit tests using NUnit, Moq, and FluentAssertions.

### Running Tests
```bash
# Run all tests
dotnet test

# Run with detailed output
dotnet test --verbosity normal

# Run with code coverage
dotnet test /p:CollectCoverage=true
```

### Test Framework

- **NUnit 4.3**: Modern .NET testing framework
- **Moq 4.20**: Mocking library for dependency injection
- **FluentAssertions**: Readable assertion library

### Test Coverage

- Services: Configuration and initialization
- Features: All analyzer and runner classes
- Presenters: Output formatting and conditional rendering
- Models: Data structure validation
## ✨ Features

### 🔍 1. Sprint Retrospective Analyzer
Transforms unstructured retrospective notes into actionable insights.

**Input:**
```
Sprint went well overall. Velocity increased from 35 to 42 points.
However, we had 3 production incidents due to insufficient testing.
Deployment process still takes 4 hours - needs automation.
Sarah did excellent work on the authentication refactor.
```

**Output:**
- Overall sentiment analysis (Positive/Neutral/Negative)
- Key themes extraction
- Prioritized action items with effort estimates
- Wins and concerns identification
- Manager-specific recommendations

**Value:** Saves ~2 hours per retrospective in manual analysis

---

### 🔎 2. Code Review Assistant
Provides architectural, security, and best practice review of C# code.

**Analyzes:**
- Architecture & design patterns
- Security vulnerabilities (SQL injection, XSS, etc.)
- Performance bottlenecks
- SOLID principles compliance
- .NET best practices
- Async/await anti-patterns

**Returns:** Severity-ranked feedback with line numbers and specific suggestions

**Value:** Catches issues humans might miss, provides consistent review standards

---

### 📊 3. Technical Debt Prioritizer
ROI-based prioritization of technical debt items.

**Evaluates:**
- Business impact vs. implementation effort
- Security and stability risks
- Dependencies between items
- Estimated effort in days
- ROI score (1-100)

**Returns:** Prioritized backlog with reasoning and strategic recommendations

**Value:** Data-driven roadmap planning, objective prioritization

---

## 🏗️ Architecture
```
┌──────────────────────┐
│   Console Interface  │
└──────────┬───────────┘
           │
     ┌─────▼──────┐
     │  Features  │ (Retrospective, CodeReview, TechDebt)
     └─────┬──────┘
           │
    ┌──────▼───────┐
    │ClaudeService │ (HTTP Client wrapper)
    └──────┬───────┘
           │
     ┌─────▼──────┐
     │ Claude API │ (Anthropic)
     └────────────┘
```

**Design Principles:**
- ✅ Clean separation of concerns
- ✅ Interface-based dependency injection
- ✅ Structured LLM responses (JSON)
- ✅ Comprehensive error handling
- ✅ Single Responsibility Principle

---

## 🚀 Getting Started

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [Anthropic API Key](https://console.anthropic.com/)

### Installation
```bash
# Clone the repository
git clone https://github.com/YOUR_USERNAME/DevTeamAIAssistant.git
cd DevTeamAIAssistant

# Restore dependencies
dotnet restore
```

### Configuration

1. **Copy the example configuration:**
```bash
   cp appsettings.example.json appsettings.json
```

2. **Add your Anthropic API key:**
   
   Edit `appsettings.json`:
```json
   {
     "Anthropic": {
       "ApiKey": "sk-ant-api03-your-actual-key-here",
       "Model": "claude-sonnet-4-20250514"
     }
   }
```

3. **Get an API key:**
   - Go to [Anthropic Console](https://console.anthropic.com/settings/keys)
   - Create a new API key
   - Copy and paste into `appsettings.json`

⚠️ **Important:** Never commit `appsettings.json` to version control. It's gitignored by default.

### Run
```bash
dotnet run
```

---

## 💡 Example Usage

### Retrospective Analysis

**Input:**
```
Team completed 45 story points (up from 35).
Sarah's refactoring of auth system was excellent.
Still seeing deployment delays - takes 3+ hours.
2 production incidents this sprint - need better testing.
Communication between frontend/backend teams improved.
```

**Output:**
```
📊 Overall Sentiment: POSITIVE

🎯 Key Themes:
  • Velocity improvement
  • Deployment automation needs
  • Production quality concerns
  • Cross-team collaboration success

✅ Wins:
  • 29% velocity increase
  • Successful authentication refactor
  • Improved team communication

⚠️ Concerns:
  • Manual deployment bottleneck
  • Production incident rate

📋 Action Items:
  [HIGH] Implement CI/CD pipeline to reduce deployment time
      Owner: DevOps Lead | Effort: 5 days
  
  [HIGH] Add pre-production smoke tests
      Owner: QA Lead | Effort: 3 days

💡 Manager Recommendation:
  Focus on automation to sustain velocity gains. Consider 
  dedicating 1 sprint to infrastructure improvements.
```

---

### Code Review

**Input:** (C# code with issues)

**Output:**
```
📊 Quality Score: 6/10

🔒 Security Concerns:
  ⚠️ SQL Injection vulnerability in GetUser method
  ⚠️ Hardcoded connection string with credentials
  ⚠️ Password exposed in API response

✅ Best Practices Observed:
  • Using 'using' statements for IDisposable
  • Async/await pattern usage

💬 Review Comments:

  [CRITICAL]
    Security: SQL injection vulnerability detected
    💡 Use parameterized queries with SqlParameter
    📍 Line 15

  [HIGH]
    Architecture: Hardcoded connection string
    💡 Move to configuration file with dependency injection
    📍 Line 8
```

---

### Technical Debt Prioritization

**Input:**
```
Legacy authentication system using MD5 hashing
Monolithic API needs microservices split
No monitoring or logging in production
Database queries not optimized
No automated backups configured
```

**Output:**
```
⚠️ Risk Assessment:
  High risk due to authentication vulnerabilities and lack 
  of production monitoring

📊 Total Estimated Effort: 25 days

🎯 Prioritized Backlog:

🔴 Priority: 10/10 | Impact: High | Effort: High
   ROI Score: 95/100
   Legacy authentication system using MD5 hashing
   💭 Critical security vulnerability affecting all users
   ⏱️ Estimated: 10 days

🟡 Priority: 8/10 | Impact: High | Effort: Medium
   ROI Score: 85/100
   No monitoring or logging in production
   💭 Cannot diagnose issues, high operational risk
   ⏱️ Estimated: 5 days
   🔗 Dependencies: None

💡 Strategic Recommendation:
  Address security vulnerabilities immediately. Authentication 
  system poses the highest risk. Plan monitoring implementation 
  in parallel to provide visibility during other improvements.
```

---

## 🎓 Learning Outcomes

This project demonstrates proficiency in:

**AI Integration:**
- ✅ LLM API integration in .NET
- ✅ Prompt engineering for structured outputs
- ✅ JSON deserialization from AI responses
- ✅ Error handling for non-deterministic systems

**Software Architecture:**
- ✅ Clean architecture principles
- ✅ Dependency injection patterns
- ✅ Interface-based design
- ✅ HTTP client best practices
- ✅ Configuration management

**Development Management:**
- ✅ Understanding of Agile workflows
- ✅ Code review automation
- ✅ Technical debt management
- ✅ Data-driven decision making

---

## 🛠️ Technology Stack

| Technology | Purpose |
|-----------|---------|
| .NET 8.0 | Runtime framework |
| C# 12 | Programming language |
| Anthropic Claude API | LLM provider |
| System.Text.Json | JSON serialization |
| HttpClient | API communication |
| Microsoft.Extensions.Configuration | App configuration |

---

## 📁 Project Structure
```
DevTeamAIAssistant/
├── Program.cs                          # Application entry point
├── Services/
│   ├── IClaudeService.cs               # Service interface
│   └── ClaudeService.cs                # Claude API integration
├── Features/
│   ├── IAnalyzer.cs                    # Generic analyzer interface
│   ├── IAnalyzerRunner.cs              # Runner interface
│   ├── IAnalyzerFactory.cs             # Factory interface
│   ├── AnalyzerBase.cs                 # Shared analyzer logic & input sanitization
│   ├── AnalyzerRunnerBase.cs           # Shared runner orchestration
│   ├── AnalyzerFactory.cs              # Feature registration & lookup
│   ├── IO/
│   │   ├── IConsoleWriter.cs
│   │   ├── IConsoleReader.cs
│   │   ├── ConsoleWriter.cs
│   │   └── ConsoleReader.cs
│   ├── Presenters/
│   │   └── IAnalyzerPresenter.cs       # Generic presenter interface
│   ├── Retrospective/
│   │   ├── RetrospectiveAnalyzer.cs
│   │   ├── RetrospectiveRunner.cs
│   │   └── RetrospectivePresenter.cs
│   ├── CodeReview/
│   │   ├── CodeReviewAnalyzer.cs
│   │   ├── CodeReviewRunner.cs
│   │   └── CodeReviewPresenter.cs
│   └── TechDebt/
│       ├── TechDebtPriorityAnalyzer.cs
│       ├── TechDebtRunner.cs
│       └── TechDebtPresenter.cs
├── Models/                             # Domain models
├── Requests/                           # Request contracts
├── Response/                           # Response contracts
├── DevTeamAIAssistant.test/
│   ├── Features/
│   │   ├── Presenters/                 # Presenter output tests
│   │   └── ...                        # Analyzer & runner tests
│   ├── Models/
│   └── Services/
├── appsettings.json                    # Config (gitignored)
├── appsettings.example.json            # Config template
├── .gitignore
├── LICENSE
└── README.md
```

---

## 📈 Future Enhancements

- [ ] **Web UI** - Blazor Server interface for browser-based access
- [ ] **Persistence** - SQLite/PostgreSQL for storing analysis history
- [ ] **Azure DevOps Integration** - Pull work items and sprint data automatically
- [ ] **Jira Integration** - Sync with existing project management tools
- [ ] **Team Dashboard** - Visualize trends over time
- [ ] **Slack/Teams Bot** - Chat interface for quick analysis
- [ ] **Multi-Model Support** - Add GPT-4, Gemini options
- [ ] **Docker Container** - Containerized deployment
- [x] **Unit Tests** - 73 tests covering analyzers, runners, presenters, models, and services
- [ ] **GitHub Actions CI/CD** - Automated build and test pipeline

---

## 🤝 Contributing

This is a portfolio/learning project, but contributions are welcome!

**Areas for improvement:**
- Additional analysis features
- Better prompt engineering
- UI enhancements
- Integration with more tools
- Performance optimizations

**To contribute:**
1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

---

## 📄 License

MIT License - See [LICENSE](LICENSE) file for details

---

## 👤 Author

**Jacques Steward**  
Software Development Manager | 10+ Years .NET/C# | AI-Augmented Development Advocate

- 💼 [LinkedIn](https://linkedin.com/in/yourprofile)
- 🌐 [Portfolio](https://yourportfolio.com)
- 📧 your.email@example.com
- 🐙 [GitHub](https://github.com/YOUR_USERNAME)

---

## 🙏 Acknowledgments

- Built with [Anthropic Claude](https://www.anthropic.com/)
- Inspired by real development management challenges
- Created as a demonstration of AI-augmented development workflows

---

## 📊 Project Stats

- **Language:** C#
- **Framework:** .NET 8.0
- **Lines of Code:** ~1,100
- **Test Count:** 73 unit tests
- **Dependencies:** 2 NuGet packages
- **Build Time:** 1-2 weeks
- **API Cost:** ~$0.50 for testing

---

## 🔐 Security Note

This project includes API key management best practices:
- API keys stored in gitignored configuration files
- Example configuration provided for setup
- Environment variable support available
- Never commit secrets to version control

---

**⭐ If you find this useful, please star the repository!**

**💬 Questions or suggestions? Open an issue or reach out on LinkedIn.**

---

*Built with ❤️ and AI assistance to demonstrate practical LLM integration in enterprise .NET applications*