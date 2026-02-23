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

  const ExamQuestionsLoaded({
    required this.questions,
    required this.attemptId,
  });

  @override
  List<Object?> get props => [questions, attemptId];
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

class ExamResultLoaded extends ExamState {
  final ExamResultModel result;

  const ExamResultLoaded({required this.result});

  @override
  List<Object?> get props => [result];
}

class QuestionReviewLoaded extends ExamState {
  final List<QuestionReviewModel> questions;

  const QuestionReviewLoaded({required this.questions});

  @override
  List<Object?> get props => [questions];
}

class ExamError extends ExamState {
  final String message;

  const ExamError({required this.message});

  @override
  List<Object?> get props => [message];
}
