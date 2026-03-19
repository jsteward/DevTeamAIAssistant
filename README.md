# Example Data Files

This folder contains sample inputs for testing the DevTeam AI Assistant features.

## Sprint Retrospective Analyzer

**File:** `retrospective-sample.txt`

Sample retrospective notes demonstrating various sentiment levels, action items, and team dynamics.

## Technical Debt Prioritizer

**File:** `tech-debt-sample.txt`

Common technical debt items found in real projects, suitable for prioritization analysis.

## Code Review Assistant

**Files:** `code-review-*.txt`

Multiple code examples demonstrating common issues:

- `code-review-sql-injection.txt` - SQL injection vulnerabilities
- `code-review-async.txt` - Async/await anti-patterns
- `code-review-solid.txt` - SOLID principle violations
- `code-review-good.txt` - Well-designed code for comparison

### How to Use

1. Run the application: `dotnet run`
2. Select a feature (e.g., "2. Review Code")
3. Copy and paste the content from the example file
4. Type `END` on a new line
5. Review the AI analysis

**Example:**
```bash
dotnet run

# Select "2. Review Code"
# Open Examples/code-review-sql-injection.txt
# Copy all contents
# Paste into terminal
# Type END
# See the analysis!
```

## Tips

- The code examples intentionally contain bugs/issues for demonstration
- Use `code-review-good.txt` to see how AI analyzes well-written code
- Mix and match examples to test different scenarios
```

---

## **ALSO: Update .gitignore**

Make sure your `.gitignore` doesn't exclude `.txt` files:
```
## .NET
bin/
obj/
*.user
*.suo
*.cache
*.log

## Build results
[Dd]ebug/
[Rr]elease/
x64/
x86/

## Configuration (contains API keys)
appsettings.json
appsettings.*.json
!appsettings.example.json

## IDE
.vs/
.vscode/
*.csproj.user

## macOS
.DS_Store

## DO NOT ignore examples
!Examples/**/*.txt
```

---

## **✅ NOW IT WORKS**

The `.cs` files were being included in compilation. By making them `.txt` files:

1. ✅ They won't be compiled
2. ✅ No dependency issues
3. ✅ Users can still copy/paste them
4. ✅ Git tracks them properly
5. ✅ Clean separation between code and examples

---

## **🎯 FINAL PROJECT STRUCTURE**
```
DevTeamAIAssistant/
├── DevTeamAIAssistant.csproj
├── Program.cs
├── appsettings.json          (gitignored)
├── appsettings.example.json
├── README.md
├── LICENSE
├── .gitignore
├── Services/
│   ├── IClaudeService.cs
│   └── ClaudeService.cs
├── Models/
│   ├── RetrospectiveReport.cs
│   ├── CodeReviewResult.cs
│   └── TechDebtItem.cs
├── Features/
│   ├── RetrospectiveAnalyzer.cs
│   ├── CodeReviewer.cs
│   └── TechDebtPrioritizer.cs
└── Examples/
    ├── README.md
    ├── retrospective-sample.txt
    ├── tech-debt-sample.txt
    ├── code-review-sql-injection.txt
    ├── code-review-async.txt
    ├── code-review-solid.txt
    └── code-review-good.txt