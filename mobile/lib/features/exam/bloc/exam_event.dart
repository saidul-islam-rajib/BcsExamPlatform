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

class LoadExamQuestions extends ExamEvent {
  final String attemptId;

  const LoadExamQuestions({required this.attemptId});

  @override
  List<Object?> get props => [attemptId];
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
    required this.timeSpentSeconds,
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

class SubmitExam extends ExamEvent {
  final String attemptId;

  const SubmitExam({required this.attemptId});

  @override
  List<Object?> get props => [attemptId];
}

class LoadExamResult extends ExamEvent {
  final String attemptId;

  const LoadExamResult({required this.attemptId});

  @override
  List<Object?> get props => [attemptId];
}

class LoadQuestionReview extends ExamEvent {
  final String attemptId;

  const LoadQuestionReview({required this.attemptId});

  @override
  List<Object?> get props => [attemptId];
}
