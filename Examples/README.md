# Example Data Files

This folder contains sample inputs for testing the DevTeam AI Assistant features.

## Sprint Retrospective Analyzer

**File:** `retrospective-sample.txt`

Sample retrospective notes demonstrating various sentiment levels, action items, and team dynamics.

## Technical Debt Prioritizer

**File:** `tech-debt-sample.txt`

Common technical debt items found in real projects, suitable for prioritization analysis.

## Code Review Assistant

**Folder:** `code-review/`

Multiple code examples demonstrating common issues:

- `sql-injection.cs` - SQL injection vulnerabilities
- `async-issues.cs` - Async/await anti-patterns
- `solid-violations.cs` - SOLID principle violations
- `good-architecture.cs` - Well-designed code for comparison

### How to Use

Copy and paste the content from these files when testing the features, or use them as templates for your own data.

**Example:**
```bash
# Run the application
dotnet run

# Select "2. Review Code"
# Copy contents from Examples/code-review/sql-injection.cs
# Paste and type END
# Review the AI analysis
```