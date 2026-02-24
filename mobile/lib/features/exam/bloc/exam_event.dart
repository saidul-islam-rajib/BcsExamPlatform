import 'package:equatable/equatable.dart';

abstract class ExamEvent extends Equatable {
  const ExamEvent();

  @override
  List<Object?> get props => [];
}

class LoadExams extends ExamEvent {
  final String language;

  const LoadExams({this.language = 'Bangla'});

  @override
  List<Object?> get props => [language];
}

// Alias for backward compatibility
class LoadExamsRequested extends LoadExams {
  const LoadExamsRequested({super.language});
}

class LoadExamDetail extends ExamEvent {
  final String examId;
  final String language;

  const LoadExamDetail({
    required this.examId,
    this.language = 'Bangla',
  });

  @override
  List<Object?> get props => [examId, language];
}

// Alias for backward compatibility
class LoadExamDetailsRequested extends LoadExamDetail {
  const LoadExamDetailsRequested({
    required super.examId,
    super.language,
  });
}

class StartExam extends ExamEvent {
  final String examId;
  final String selectedLanguage;
  final String? guestEmail;

  const StartExam({
    required this.examId,
    required this.selectedLanguage,
    this.guestEmail,
  });

  @override
  List<Object?> get props => [examId, selectedLanguage, guestEmail];
}

// Alias for backward compatibility
class StartExamRequested extends StartExam {
  const StartExamRequested({
    required super.examId,
    required super.selectedLanguage,
    super.guestEmail,
  });
}

class LoadExamQuestions extends ExamEvent {
  final String attemptId;

  const LoadExamQuestions({required this.attemptId});

  @override
  List<Object?> get props => [attemptId];
}

// Alias for backward compatibility
class LoadExamQuestionsRequested extends LoadExamQuestions {
  const LoadExamQuestionsRequested({required super.attemptId});
}

class SubmitAnswer extends ExamEvent {
  final String attemptId;
  final String questionId;
  final String? selectedOptionId;
  final int timeSpentSeconds;
  final bool isMarkedForReview;

  const SubmitAnswer({
    required this.attemptId,
    required this.questionId,
    this.selectedOptionId,
    this.timeSpentSeconds = 0,
    this.isMarkedForReview = false,
  });

  @override
  List<Object?> get props => [
        attemptId,
        questionId,
        selectedOptionId,
        timeSpentSeconds,
        isMarkedForReview,
      ];
}

// Alias for backward compatibility
class SubmitAnswerRequested extends SubmitAnswer {
  const SubmitAnswerRequested({
    required super.attemptId,
    required super.questionId,
    super.selectedOptionId,
    super.timeSpentSeconds,
    super.isMarkedForReview,
  });
}

class SubmitExam extends ExamEvent {
  final String attemptId;

  const SubmitExam({required this.attemptId});

  @override
  List<Object?> get props => [attemptId];
}

// Alias for backward compatibility
class SubmitExamRequested extends SubmitExam {
  const SubmitExamRequested({required super.attemptId});
}

class LoadExamResult extends ExamEvent {
  final String attemptId;

  const LoadExamResult({required this.attemptId});

  @override
  List<Object?> get props => [attemptId];
}

// Alias for backward compatibility
class LoadResultRequested extends LoadExamResult {
  const LoadResultRequested({required super.attemptId});
}

class LoadQuestionReview extends ExamEvent {
  final String attemptId;

  const LoadQuestionReview({required this.attemptId});

  @override
  List<Object?> get props => [attemptId];
}

// Alias for backward compatibility
class LoadReviewRequested extends LoadQuestionReview {
  const LoadReviewRequested({required super.attemptId});
}
