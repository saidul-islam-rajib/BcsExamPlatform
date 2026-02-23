import 'package:equatable/equatable.dart';
import '../../../core/models/exam_model.dart';
import '../../../core/models/question_model.dart';
import '../../../core/models/result_model.dart';

abstract class ExamState extends Equatable {
  const ExamState();

  @override
  List<Object?> get props => [];
}

class ExamInitial extends ExamState {}

class ExamLoading extends ExamState {}

class ExamsLoaded extends ExamState {
  final List<ExamModel> exams;

  const ExamsLoaded({required this.exams});

  @override
  List<Object?> get props => [exams];
}

class ExamDetailsLoaded extends ExamState {
  final ExamModel exam;
  final List<SubjectDistribution> subjectDistributions;

  const ExamDetailsLoaded({
    required this.exam,
    required this.subjectDistributions,
  });

  @override
  List<Object?> get props => [exam, subjectDistributions];
}

class ExamDetailLoaded extends ExamState {
  final ExamDetailModel exam;

  const ExamDetailLoaded({required this.exam});

  @override
  List<Object?> get props => [exam];
}

class ExamStarted extends ExamState {
  final String attemptId;
  final DateTime startedAt;

  const ExamStarted({
    required this.attemptId,
    required this.startedAt,
  });

  @override
  List<Object?> get props => [attemptId, startedAt];
}

class ExamQuestionsLoaded extends ExamState {
  final List<QuestionModel> questions;
  final String attemptId;
  final int durationMinutes;

  const ExamQuestionsLoaded({
    required this.questions,
    required this.attemptId,
    required this.durationMinutes,
  });

  @override
  List<Object?> get props => [questions, attemptId, durationMinutes];
}

class AnswerSubmitted extends ExamState {
  final String message;

  const AnswerSubmitted({required this.message});

  @override
  List<Object?> get props => [message];
}

class ExamSubmitted extends ExamState {
  final ExamResultModel result;

  const ExamSubmitted({required this.result});

  @override
  List<Object?> get props => [result];
}

class ResultLoaded extends ExamState {
  final ResultModel result;

  const ResultLoaded({required this.result});

  @override
  List<Object?> get props => [result];
}

class ExamResultLoaded extends ExamState {
  final ExamResultModel result;

  const ExamResultLoaded({required this.result});

  @override
  List<Object?> get props => [result];
}

class ReviewLoaded extends ExamState {
  final List<ReviewAnswer> answers;

  const ReviewLoaded({required this.answers});

  @override
  List<Object?> get props => [answers];
}

class QuestionReviewLoaded extends ExamState {
  final List<QuestionReviewModel> questions;

  const QuestionReviewLoaded({required this.questions});

  @override
  List<Object?> get props => [questions];
}

class ExamFailure extends ExamState {
  final String error;

  const ExamFailure({required this.error});

  @override
  List<Object?> get props => [error];
}

class ExamError extends ExamState {
  final String message;

  const ExamError({required this.message});

  @override
  List<Object?> get props => [message];
}

// Helper classes for state data
class SubjectDistribution {
  final String subjectNameEnglish;
  final String subjectNameBangla;
  final int totalQuestions;
  final int easyQuestions;
  final int intermediateQuestions;
  final int hardQuestions;

  SubjectDistribution({
    required this.subjectNameEnglish,
    required this.subjectNameBangla,
    required this.totalQuestions,
    required this.easyQuestions,
    required this.intermediateQuestions,
    required this.hardQuestions,
  });
}

class ReviewAnswer {
  final QuestionModel question;
  final String? selectedOptionId;
  final bool isCorrect;

  ReviewAnswer({
    required this.question,
    this.selectedOptionId,
    required this.isCorrect,
  });
}
