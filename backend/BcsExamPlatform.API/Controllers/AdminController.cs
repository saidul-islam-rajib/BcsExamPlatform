using BcsExamPlatform.API.DTOs;
using BcsExamPlatform.Core.Entities;
using BcsExamPlatform.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BcsExamPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly BcsExamPlatform.Core.Services.IOpenAIService? _openAIService;

    public AdminController(ApplicationDbContext context, BcsExamPlatform.Core.Services.IOpenAIService? openAIService = null)
    {
        _context = context;
        _openAIService = openAIService;
    }

    // Question Management

    [HttpPost("questions")]
    public async Task<ActionResult<QuestionResponseDTO>> CreateQuestion(CreateQuestionDTO dto)
    {
        var question = new Question
        {
            QuestionId = Guid.NewGuid(),
            SubjectId = dto.SubjectId,
            TopicId = dto.TopicId,
            QuestionTextBangla = dto.QuestionTextBangla,
            QuestionTextEnglish = dto.QuestionTextEnglish,
            QuestionImageUrl = dto.QuestionImageUrl,
            HasMathContent = dto.HasMathContent,
            DifficultyLevel = dto.DifficultyLevel,
            Marks = dto.Marks,
            SourceType = dto.SourceType,
            SourceYear = dto.SourceYear,
            SourceReference = dto.SourceReference,
            IsAIGenerated = dto.IsAIGenerated,
            IsUnique = dto.IsUnique,
            IsApproved = dto.IsApproved,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Questions.Add(question);
        await _context.SaveChangesAsync();

        return Ok(new QuestionResponseDTO
        {
            QuestionId = question.QuestionId,
            Message = "Question created successfully"
        });
    }

    [HttpPost("questions/{questionId}/options")]
    public async Task<ActionResult> AddQuestionOptions(Guid questionId, List<CreateQuestionOptionDTO> options)
    {
        var question = await _context.Questions.FindAsync(questionId);
        if (question == null)
        {
            return NotFound(new { message = "Question not found" });
        }

        var questionOptions = options.Select((opt, index) => new QuestionOption
        {
            OptionId = Guid.NewGuid(),
            QuestionId = questionId,
            OptionTextBangla = opt.OptionTextBangla,
            OptionTextEnglish = opt.OptionTextEnglish,
            OptionOrder = index + 1,
            IsCorrect = opt.IsCorrect,
            CreatedAt = DateTime.UtcNow
        }).ToList();

        _context.QuestionOptions.AddRange(questionOptions);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Options added successfully" });
    }

    [HttpPost("questions/{questionId}/explanation")]
    public async Task<ActionResult> AddQuestionExplanation(Guid questionId, CreateExplanationDTO dto)
    {
        var question = await _context.Questions.FindAsync(questionId);
        if (question == null)
        {
            return NotFound(new { message = "Question not found" });
        }

        var explanation = new QuestionExplanation
        {
            ExplanationId = Guid.NewGuid(),
            QuestionId = questionId,
            ExplanationBangla = dto.ExplanationBangla,
            ExplanationEnglish = dto.ExplanationEnglish,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.QuestionExplanations.AddRange(explanation);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Explanation added successfully" });
    }

    [HttpGet("questions")]
    public async Task<ActionResult<List<QuestionListDTO>>> GetQuestions(
        [FromQuery] Guid? subjectId = null,
        [FromQuery] string? difficulty = null,
        [FromQuery] bool? isApproved = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        var query = _context.Questions
            .Include(q => q.Subject)
            .Include(q => q.Topic)
            .Include(q => q.Options)
            .AsQueryable();

        if (subjectId.HasValue)
        {
            query = query.Where(q => q.SubjectId == subjectId.Value);
        }

        if (!string.IsNullOrEmpty(difficulty))
        {
            query = query.Where(q => q.DifficultyLevel == difficulty);
        }

        if (isApproved.HasValue)
        {
            query = query.Where(q => q.IsApproved == isApproved.Value);
        }

        var totalCount = await query.CountAsync();
        var questions = await query
            .OrderByDescending(q => q.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var questionDTOs = questions.Select(q => new QuestionListDTO
        {
            QuestionId = q.QuestionId,
            QuestionTextEnglish = q.QuestionTextEnglish,
            SubjectName = q.Subject.SubjectNameEnglish,
            TopicName = q.Topic.TopicNameEnglish,
            DifficultyLevel = q.DifficultyLevel,
            IsApproved = q.IsApproved,
            OptionsCount = q.Options.Count,
            CreatedAt = q.CreatedAt
        }).ToList();

        return Ok(new
        {
            totalCount,
            page,
            pageSize,
            questions = questionDTOs
        });
    }

    [HttpGet("questions/{questionId}")]
    public async Task<ActionResult<QuestionDetailDTO>> GetQuestionDetail(Guid questionId)
    {
        var question = await _context.Questions
            .Include(q => q.Subject)
            .Include(q => q.Topic)
            .Include(q => q.Options)
            .Include(q => q.Explanation)
            .FirstOrDefaultAsync(q => q.QuestionId == questionId);

        if (question == null)
        {
            return NotFound(new { message = "Question not found" });
        }

        var dto = new QuestionDetailDTO
        {
            QuestionId = question.QuestionId,
            SubjectId = question.SubjectId,
            TopicId = question.TopicId,
            QuestionTextBangla = question.QuestionTextBangla,
            QuestionTextEnglish = question.QuestionTextEnglish,
            QuestionImageUrl = question.QuestionImageUrl,
            HasMathContent = question.HasMathContent,
            DifficultyLevel = question.DifficultyLevel,
            Marks = question.Marks,
            SourceType = question.SourceType,
            SourceYear = question.SourceYear,
            SourceReference = question.SourceReference,
            IsAIGenerated = question.IsAIGenerated,
            IsApproved = question.IsApproved,
            Options = question.Options.Select(o => new QuestionOptionDetailDTO
            {
                OptionId = o.OptionId,
                OptionTextBangla = o.OptionTextBangla,
                OptionTextEnglish = o.OptionTextEnglish,
                OptionOrder = o.OptionOrder,
                IsCorrect = o.IsCorrect
            }).ToList(),
            ExplanationBangla = question.Explanation?.ExplanationBangla ?? "",
            ExplanationEnglish = question.Explanation?.ExplanationEnglish ?? ""
        };

        return Ok(dto);
    }

    [HttpPut("questions/{questionId}")]
    public async Task<ActionResult> UpdateQuestion(Guid questionId, UpdateQuestionDTO dto)
    {
        var question = await _context.Questions.FindAsync(questionId);
        if (question == null)
        {
            return NotFound(new { message = "Question not found" });
        }

        question.SubjectId = dto.SubjectId;
        question.TopicId = dto.TopicId;
        question.QuestionTextBangla = dto.QuestionTextBangla;
        question.QuestionTextEnglish = dto.QuestionTextEnglish;
        question.QuestionImageUrl = dto.QuestionImageUrl;
        question.HasMathContent = dto.HasMathContent;
        question.DifficultyLevel = dto.DifficultyLevel;
        question.Marks = dto.Marks;
        question.SourceType = dto.SourceType;
        question.SourceYear = dto.SourceYear;
        question.SourceReference = dto.SourceReference;
        question.IsApproved = dto.IsApproved;
        question.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new { message = "Question updated successfully" });
    }

    [HttpDelete("questions/{questionId}")]
    public async Task<ActionResult> DeleteQuestion(Guid questionId)
    {
        var question = await _context.Questions
            .Include(q => q.Options)
            .Include(q => q.Explanation)
            .FirstOrDefaultAsync(q => q.QuestionId == questionId);

        if (question == null)
        {
            return NotFound(new { message = "Question not found" });
        }

        _context.Questions.Remove(question);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Question deleted successfully" });
    }

    // Exam Management

    [HttpPost("exams")]
    public async Task<ActionResult<ExamResponseDTO>> CreateExam(CreateExamDTO dto)
    {
        var exam = new Exam
        {
            ExamId = Guid.NewGuid(),
            ExamNameBangla = dto.ExamNameBangla,
            ExamNameEnglish = dto.ExamNameEnglish,
            ExamDate = dto.ExamDate,
            TotalQuestions = dto.TotalQuestions,
            DurationMinutes = dto.DurationMinutes,
            TotalMarks = dto.TotalMarks,
            LanguageMode = dto.LanguageMode,
            IsPaid = dto.IsPaid,
            ExamFee = dto.ExamFee,
            AllowGuestUsers = dto.AllowGuestUsers,
            IsPublished = false,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Exams.Add(exam);
        await _context.SaveChangesAsync();

        return Ok(new ExamResponseDTO
        {
            ExamId = exam.ExamId,
            Message = "Exam created successfully"
        });
    }

    [HttpPost("exams/{examId}/subject-distribution")]
    public async Task<ActionResult> AddSubjectDistribution(Guid examId, List<CreateSubjectDistributionDTO> distributions)
    {
        var exam = await _context.Exams.FindAsync(examId);
        if (exam == null)
        {
            return NotFound(new { message = "Exam not found" });
        }

        var subjectDistributions = distributions.Select(d => new ExamSubjectDistribution
        {
            DistributionId = Guid.NewGuid(),
            ExamId = examId,
            SubjectId = d.SubjectId,
            TotalQuestions = d.TotalQuestions,
            EasyQuestions = d.EasyQuestions,
            IntermediateQuestions = d.IntermediateQuestions,
            HardQuestions = d.HardQuestions
        }).ToList();

        _context.ExamSubjectDistributions.AddRange(subjectDistributions);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Subject distribution added successfully" });
    }

    [HttpPost("exams/{examId}/generate-questions")]
    public async Task<ActionResult> GenerateExamQuestions(Guid examId)
    {
        var exam = await _context.Exams
            .Include(e => e.SubjectDistributions)
            .FirstOrDefaultAsync(e => e.ExamId == examId);

        if (exam == null)
        {
            return NotFound(new { message = "Exam not found" });
        }

        // Remove existing exam questions
        var existingQuestions = await _context.ExamQuestions
            .Where(eq => eq.ExamId == examId)
            .ToListAsync();
        _context.ExamQuestions.RemoveRange(existingQuestions);

        int questionNumber = 1;
        var examQuestions = new List<ExamQuestion>();

        foreach (var distribution in exam.SubjectDistributions)
        {
            // Get easy questions
            var easyQuestions = await _context.Questions
                .Where(q => q.SubjectId == distribution.SubjectId)
                .Where(q => q.DifficultyLevel == "Easy")
                .Where(q => q.IsApproved && q.IsActive)
                .OrderBy(q => Guid.NewGuid())
                .Take(distribution.EasyQuestions)
                .ToListAsync();

            // Get intermediate questions
            var intermediateQuestions = await _context.Questions
                .Where(q => q.SubjectId == distribution.SubjectId)
                .Where(q => q.DifficultyLevel == "Intermediate")
                .Where(q => q.IsApproved && q.IsActive)
                .OrderBy(q => Guid.NewGuid())
                .Take(distribution.IntermediateQuestions)
                .ToListAsync();

            // Get hard questions
            var hardQuestions = await _context.Questions
                .Where(q => q.SubjectId == distribution.SubjectId)
                .Where(q => q.DifficultyLevel == "Hard")
                .Where(q => q.IsApproved && q.IsActive)
                .OrderBy(q => Guid.NewGuid())
                .Take(distribution.HardQuestions)
                .ToListAsync();

            var allSubjectQuestions = easyQuestions
                .Concat(intermediateQuestions)
                .Concat(hardQuestions)
                .ToList();

            if (allSubjectQuestions.Count < distribution.TotalQuestions)
            {
                return BadRequest(new
                {
                    message = $"Insufficient questions for subject. Required: {distribution.TotalQuestions}, Available: {allSubjectQuestions.Count}"
                });
            }

            foreach (var question in allSubjectQuestions)
            {
                examQuestions.Add(new ExamQuestion
                {
                    ExamQuestionId = Guid.NewGuid(),
                    ExamId = examId,
                    QuestionId = question.QuestionId,
                    QuestionNumber = questionNumber++,
                    DisplayOrder = questionNumber
                });
            }
        }

        _context.ExamQuestions.AddRange(examQuestions);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Exam questions generated successfully",
            totalQuestions = examQuestions.Count
        });
    }

    [HttpPut("exams/{examId}/publish")]
    public async Task<ActionResult> PublishExam(Guid examId)
    {
        var exam = await _context.Exams.FindAsync(examId);
        if (exam == null)
        {
            return NotFound(new { message = "Exam not found" });
        }

        exam.IsPublished = true;
        exam.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Ok(new { message = "Exam published successfully" });
    }

    [HttpPut("exams/{examId}/unpublish")]
    public async Task<ActionResult> UnpublishExam(Guid examId)
    {
        var exam = await _context.Exams.FindAsync(examId);
        if (exam == null)
        {
            return NotFound(new { message = "Exam not found" });
        }

        exam.IsPublished = false;
        exam.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Ok(new { message = "Exam unpublished successfully" });
    }


    [HttpGet("exams")]
    public async Task<ActionResult<List<ExamListAdminDTO>>> GetAllExams()
    {
        var exams = await _context.Exams
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync();

        var examDTOs = exams.Select(e => new ExamListAdminDTO
        {
            ExamId = e.ExamId,
            ExamNameEnglish = e.ExamNameEnglish,
            ExamDate = e.ExamDate,
            TotalQuestions = e.TotalQuestions,
            DurationMinutes = e.DurationMinutes,
            IsPublished = e.IsPublished,
            IsPaid = e.IsPaid,
            ExamFee = e.ExamFee ?? 0,
            CreatedAt = e.CreatedAt
        }).ToList();

        return Ok(examDTOs);
    }

    [HttpGet("exams/{examId}/questions")]
    public async Task<ActionResult> GetExamQuestions(Guid examId)
    {
        var exam = await _context.Exams.FindAsync(examId);
        if (exam == null)
        {
            return NotFound(new { message = "Exam not found" });
        }

        var examQuestions = await _context.ExamQuestions
            .Where(eq => eq.ExamId == examId)
            .Include(eq => eq.Question)
                .ThenInclude(q => q.Subject)
            .Include(eq => eq.Question)
                .ThenInclude(q => q.Topic)
            .OrderBy(eq => eq.QuestionNumber)
            .Select(eq => new
            {
                questionId = eq.QuestionId,
                questionNumber = eq.QuestionNumber,
                questionTextEnglish = eq.Question.QuestionTextEnglish,
                questionTextBangla = eq.Question.QuestionTextBangla,
                subjectName = eq.Question.Subject.SubjectNameEnglish,
                topicName = eq.Question.Topic.TopicNameEnglish,
                difficultyLevel = eq.Question.DifficultyLevel,
                marks = eq.Question.Marks
            })
            .ToListAsync();

        return Ok(new
        {
            examId = examId,
            examName = exam.ExamNameEnglish,
            totalQuestions = examQuestions.Count,
            questions = examQuestions
        });
    }

    // AI Question Generation (for general question bank)
    [HttpPost("questions/generate-ai")]
    public async Task<ActionResult> GenerateAIQuestions([FromBody] GenerateAIQuestionsDTO dto)
    {
        if (_openAIService == null)
        {
            return BadRequest(new { message = "OpenAI service is not configured. Please add your API key to appsettings.Development.json" });
        }

        try
        {
            var subject = await _context.Subjects.FindAsync(dto.SubjectId);
            var topic = await _context.Topics.FindAsync(dto.TopicId);

            if (subject == null || topic == null)
            {
                return BadRequest(new { message = "Invalid subject or topic" });
            }

            // Generate questions using OpenAI
            var generatedQuestions = await _openAIService.GenerateQuestionsAsync(
                subject.SubjectNameEnglish,
                topic.TopicNameEnglish,
                dto.DifficultyLevel,
                dto.Count,
                dto.Language
            );

            if (generatedQuestions.Count == 0)
            {
                return BadRequest(new { message = "Failed to generate questions. Please try again." });
            }

            // Save questions to database
            var savedQuestions = new List<Guid>();
            foreach (var genQ in generatedQuestions)
            {
                var question = new Question
                {
                    QuestionId = Guid.NewGuid(),
                    SubjectId = dto.SubjectId,
                    TopicId = dto.TopicId,
                    QuestionTextBangla = dto.Language == "Bangla" ? genQ.QuestionText : "",
                    QuestionTextEnglish = dto.Language == "English" ? genQ.QuestionText : genQ.QuestionText,
                    DifficultyLevel = dto.DifficultyLevel,
                    Marks = 1.00m,
                    SourceType = "AI Generated",
                    IsAIGenerated = true,
                    IsApproved = dto.AutoApprove,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                // Add options
                question.Options = genQ.Options.Select((opt, index) => new QuestionOption
                {
                    OptionId = Guid.NewGuid(),
                    QuestionId = question.QuestionId,
                    OptionTextBangla = dto.Language == "Bangla" ? opt : "",
                    OptionTextEnglish = dto.Language == "English" ? opt : opt,
                    OptionOrder = index + 1,
                    IsCorrect = index == genQ.CorrectOptionIndex,
                    CreatedAt = DateTime.UtcNow
                }).ToList();

                // Add explanation
                question.Explanation = new QuestionExplanation
                {
                    ExplanationId = Guid.NewGuid(),
                    QuestionId = question.QuestionId,
                    ExplanationBangla = dto.Language == "Bangla" ? genQ.Explanation : "",
                    ExplanationEnglish = dto.Language == "English" ? genQ.Explanation : genQ.Explanation,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Questions.Add(question);
                savedQuestions.Add(question.QuestionId);
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = $"Successfully generated {savedQuestions.Count} AI questions",
                questionIds = savedQuestions,
                count = savedQuestions.Count
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = $"Error generating AI questions: {ex.Message}" });
        }
    }

    // AI Question Generation for Specific Exam
    [HttpPost("exams/{examId}/generate-ai-questions")]
    public async Task<ActionResult> GenerateAIQuestionsForExam(Guid examId)
    {
        if (_openAIService == null)
        {
            return BadRequest(new { message = "OpenAI service is not configured. Please add your API key to appsettings.Development.json" });
        }

        try
        {
            var exam = await _context.Exams
                .Include(e => e.SubjectDistributions)
                .FirstOrDefaultAsync(e => e.ExamId == examId);

            if (exam == null)
            {
                return NotFound(new { message = "Exam not found" });
            }

            if (exam.SubjectDistributions == null || !exam.SubjectDistributions.Any())
            {
                return BadRequest(new { message = "Exam has no subject distribution defined" });
            }

            // Remove existing exam questions
            var existingQuestions = await _context.ExamQuestions
                .Where(eq => eq.ExamId == examId)
                .ToListAsync();
            _context.ExamQuestions.RemoveRange(existingQuestions);

            int questionNumber = 1;
            var examQuestions = new List<ExamQuestion>();
            var totalGenerated = 0;

            // Generate questions for each subject distribution
            foreach (var distribution in exam.SubjectDistributions)
            {
                var subject = await _context.Subjects.FindAsync(distribution.SubjectId);
                if (subject == null) continue;

                // Get a random topic for this subject
                var topic = await _context.Topics
                    .Where(t => t.SubjectId == distribution.SubjectId && t.IsActive)
                    .OrderBy(t => Guid.NewGuid())
                    .FirstOrDefaultAsync();

                if (topic == null)
                {
                    return BadRequest(new { message = $"No topics found for subject: {subject.SubjectNameEnglish}" });
                }

                // Generate Easy questions
                if (distribution.EasyQuestions > 0)
                {
                    var easyQuestions = await _openAIService.GenerateQuestionsAsync(
                        subject.SubjectNameEnglish,
                        topic.TopicNameEnglish,
                        "Easy",
                        distribution.EasyQuestions,
                        exam.LanguageMode == "Bangla" ? "Bangla" : "English"
                    );

                    foreach (var genQ in easyQuestions)
                    {
                        var question = await SaveGeneratedQuestion(genQ, distribution.SubjectId, topic.TopicId, "Easy", exam.LanguageMode);
                        examQuestions.Add(new ExamQuestion
                        {
                            ExamQuestionId = Guid.NewGuid(),
                            ExamId = examId,
                            QuestionId = question.QuestionId,
                            QuestionNumber = questionNumber++,
                            DisplayOrder = questionNumber
                        });
                        totalGenerated++;
                    }
                }

                // Generate Intermediate questions
                if (distribution.IntermediateQuestions > 0)
                {
                    var intermediateQuestions = await _openAIService.GenerateQuestionsAsync(
                        subject.SubjectNameEnglish,
                        topic.TopicNameEnglish,
                        "Intermediate",
                        distribution.IntermediateQuestions,
                        exam.LanguageMode == "Bangla" ? "Bangla" : "English"
                    );

                    foreach (var genQ in intermediateQuestions)
                    {
                        var question = await SaveGeneratedQuestion(genQ, distribution.SubjectId, topic.TopicId, "Intermediate", exam.LanguageMode);
                        examQuestions.Add(new ExamQuestion
                        {
                            ExamQuestionId = Guid.NewGuid(),
                            ExamId = examId,
                            QuestionId = question.QuestionId,
                            QuestionNumber = questionNumber++,
                            DisplayOrder = questionNumber
                        });
                        totalGenerated++;
                    }
                }

                // Generate Hard questions
                if (distribution.HardQuestions > 0)
                {
                    var hardQuestions = await _openAIService.GenerateQuestionsAsync(
                        subject.SubjectNameEnglish,
                        topic.TopicNameEnglish,
                        "Hard",
                        distribution.HardQuestions,
                        exam.LanguageMode == "Bangla" ? "Bangla" : "English"
                    );

                    foreach (var genQ in hardQuestions)
                    {
                        var question = await SaveGeneratedQuestion(genQ, distribution.SubjectId, topic.TopicId, "Hard", exam.LanguageMode);
                        examQuestions.Add(new ExamQuestion
                        {
                            ExamQuestionId = Guid.NewGuid(),
                            ExamId = examId,
                            QuestionId = question.QuestionId,
                            QuestionNumber = questionNumber++,
                            DisplayOrder = questionNumber
                        });
                        totalGenerated++;
                    }
                }
            }

            _context.ExamQuestions.AddRange(examQuestions);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "AI questions generated and assigned to exam successfully",
                totalQuestions = totalGenerated,
                examId = examId
            });
        }
        catch (Exception ex)
        {
            // Log the full exception for debugging
            Console.WriteLine($"AI Generation Error: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            
            // Return user-friendly error message
            var errorMessage = ex.Message;
            if (ex.InnerException != null)
            {
                errorMessage += $" Inner: {ex.InnerException.Message}";
            }
            
            return BadRequest(new { 
                message = $"Error generating AI questions: {errorMessage}",
                details = ex.GetType().Name
            });
        }
    }

    private async Task<Question> SaveGeneratedQuestion(
        BcsExamPlatform.Core.Services.GeneratedQuestion genQ,
        Guid subjectId,
        Guid topicId,
        string difficulty,
        string languageMode)
    {
        var question = new Question
        {
            QuestionId = Guid.NewGuid(),
            SubjectId = subjectId,
            TopicId = topicId,
            QuestionTextBangla = languageMode == "Bangla" ? genQ.QuestionText : "",
            QuestionTextEnglish = languageMode == "English" || languageMode == "Bilingual" ? genQ.QuestionText : genQ.QuestionText,
            DifficultyLevel = difficulty,
            Marks = 1.00m,
            SourceType = "AI Generated",
            IsAIGenerated = true,
            IsApproved = true, // Auto-approve for exam generation
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Add options
        question.Options = genQ.Options.Select((opt, index) => new QuestionOption
        {
            OptionId = Guid.NewGuid(),
            QuestionId = question.QuestionId,
            OptionTextBangla = languageMode == "Bangla" ? opt : "",
            OptionTextEnglish = languageMode == "English" || languageMode == "Bilingual" ? opt : opt,
            OptionOrder = index + 1,
            IsCorrect = index == genQ.CorrectOptionIndex,
            CreatedAt = DateTime.UtcNow
        }).ToList();

        // Add explanation
        question.Explanation = new QuestionExplanation
        {
            ExplanationId = Guid.NewGuid(),
            QuestionId = question.QuestionId,
            ExplanationBangla = languageMode == "Bangla" ? genQ.Explanation : "",
            ExplanationEnglish = languageMode == "English" || languageMode == "Bilingual" ? genQ.Explanation : genQ.Explanation,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Questions.Add(question);
        return question;
    }

    // Seed Sample Data
    [HttpPost("seed-sample-data")]
    public async Task<ActionResult> SeedSampleData([FromQuery] bool force = false)
    {
        try
        {
            await SeedSampleQuestions(force);
            return Ok(new { message = "Sample data seeded successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = $"Error seeding data: {ex.Message}" });
        }
    }

    // Get Subjects
    [HttpGet("subjects")]
    public async Task<ActionResult> GetSubjects()
    {
        var subjects = await _context.Subjects
            .Where(s => s.IsActive)
            .OrderBy(s => s.DisplayOrder)
            .Select(s => new
            {
                s.SubjectId,
                s.SubjectNameBangla,
                s.SubjectNameEnglish,
                s.DisplayOrder
            })
            .ToListAsync();

        return Ok(subjects);
    }

    // Get Topics by Subject
    [HttpGet("subjects/{subjectId}/topics")]
    public async Task<ActionResult> GetTopicsBySubject(Guid subjectId)
    {
        var topics = await _context.Topics
            .Where(t => t.SubjectId == subjectId && t.IsActive)
            .Select(t => new
            {
                t.TopicId,
                t.TopicNameBangla,
                t.TopicNameEnglish
            })
            .ToListAsync();

        return Ok(topics);
    }

    private async Task SeedSampleQuestions(bool force = false)
    {
        // Check if questions already exist
        var existingCount = await _context.Questions.CountAsync();
        
        if (existingCount > 0 && !force)
        {
            return; // Questions already exist, skip seeding
        }

        // If force is true and questions exist, delete them first
        if (force && existingCount > 0)
        {
            var existingQuestions = await _context.Questions
                .Include(q => q.Options)
                .Include(q => q.Explanation)
                .ToListAsync();
            _context.Questions.RemoveRange(existingQuestions);
            await _context.SaveChangesAsync();
        }

        var subjects = await _context.Subjects.ToListAsync();
        var topics = await _context.Topics.ToListAsync();

        var banglaSubject = subjects.First(s => s.SubjectNameEnglish == "Bangla");
        var englishSubject = subjects.First(s => s.SubjectNameEnglish == "English");
        var mathSubject = subjects.First(s => s.SubjectNameEnglish == "Mathematical Reasoning");

        var banglaGrammarTopic = topics.First(t => t.SubjectId == banglaSubject.SubjectId && t.TopicNameEnglish == "Grammar");
        var englishGrammarTopic = topics.First(t => t.SubjectId == englishSubject.SubjectId && t.TopicNameEnglish == "Grammar");
        var arithmeticTopic = topics.First(t => t.SubjectId == mathSubject.SubjectId && t.TopicNameEnglish == "Arithmetic");

        var questions = new List<Question>();

        // Add 50 Bangla questions
        for (int i = 1; i <= 50; i++)
        {
            var difficulty = i <= 20 ? "Easy" : (i <= 40 ? "Intermediate" : "Hard");
            var q = CreateSampleQuestion(banglaSubject.SubjectId, banglaGrammarTopic.TopicId, i, "Bangla", difficulty);
            questions.Add(q);
        }

        // Add 50 English questions
        for (int i = 1; i <= 50; i++)
        {
            var difficulty = i <= 20 ? "Easy" : (i <= 40 ? "Intermediate" : "Hard");
            var q = CreateSampleQuestion(englishSubject.SubjectId, englishGrammarTopic.TopicId, i, "English", difficulty);
            questions.Add(q);
        }

        // Add 100 Math questions
        for (int i = 1; i <= 100; i++)
        {
            var difficulty = i <= 40 ? "Easy" : (i <= 80 ? "Intermediate" : "Hard");
            var q = CreateSampleQuestion(mathSubject.SubjectId, arithmeticTopic.TopicId, i, "Math", difficulty);
            questions.Add(q);
        }

        await _context.Questions.AddRangeAsync(questions);
        await _context.SaveChangesAsync();
    }

    private Question CreateSampleQuestion(Guid subjectId, Guid topicId, int number, string subject, string difficulty)
    {
        var question = new Question
        {
            QuestionId = Guid.NewGuid(),
            SubjectId = subjectId,
            TopicId = topicId,
            QuestionTextBangla = $"{subject} প্রশ্ন নং {number} - {difficulty}",
            QuestionTextEnglish = $"{subject} Question {number} - {difficulty}",
            DifficultyLevel = difficulty,
            Marks = 1.00m,
            SourceType = "Sample",
            IsApproved = true,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        question.Options = new List<QuestionOption>
        {
            new QuestionOption { OptionId = Guid.NewGuid(), QuestionId = question.QuestionId, OptionTextBangla = "বিকল্প ১", OptionTextEnglish = "Option 1", OptionOrder = 1, IsCorrect = true, CreatedAt = DateTime.UtcNow },
            new QuestionOption { OptionId = Guid.NewGuid(), QuestionId = question.QuestionId, OptionTextBangla = "বিকল্প ২", OptionTextEnglish = "Option 2", OptionOrder = 2, IsCorrect = false, CreatedAt = DateTime.UtcNow },
            new QuestionOption { OptionId = Guid.NewGuid(), QuestionId = question.QuestionId, OptionTextBangla = "বিকল্প ৩", OptionTextEnglish = "Option 3", OptionOrder = 3, IsCorrect = false, CreatedAt = DateTime.UtcNow },
            new QuestionOption { OptionId = Guid.NewGuid(), QuestionId = question.QuestionId, OptionTextBangla = "বিকল্প ৪", OptionTextEnglish = "Option 4", OptionOrder = 4, IsCorrect = false, CreatedAt = DateTime.UtcNow }
        };

        question.Explanation = new QuestionExplanation
        {
            ExplanationId = Guid.NewGuid(),
            QuestionId = question.QuestionId,
            ExplanationBangla = $"এটি {subject} বিষয়ের একটি নমুনা প্রশ্ন।",
            ExplanationEnglish = $"This is a sample question for {subject}.",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        return question;
    }
}
