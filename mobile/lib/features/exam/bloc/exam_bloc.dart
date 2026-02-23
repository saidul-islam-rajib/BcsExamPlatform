import 'package:flutter_bloc/flutter_bloc.dart';
import '../../../core/config/app_config.dart';
import '../../../core/models/exam_model.dart';
import '../../../core/models/question_model.dart';
import '../../../core/models/result_model.dart';
import '../../../core/services/api_service.dart';
import 'exam_event.dart';
import 'exam_state.dart';

class ExamBloc extends Bloc<ExamEvent, ExamState> {
  final ApiService apiService;

  ExamBloc({required this.apiService}) : super(ExamInitial()) {
    on<LoadExams>(_onLoadExams);
    on<LoadExamDetail>(_onLoadExamDetail);
    on<StartExam>(_onStartExam);
    on<LoadExamQuestions>(_onLoadExamQuestions);
    on<SubmitAnswer>(_onSubmitAnswer);
    on<SubmitExam>(_onSubmitExam);
    on<LoadExamResult>(_onLoadExamResult);
    on<LoadQuestionReview>(_onLoadQuestionReview);
  }

  Future<void> _onLoadExams(LoadExams event, Emitter<ExamState> emit) async {
    emit(ExamLoading());

    final response = await apiService.get(
      AppConfig.examList,
      queryParams: {'language': event.language},
    );

    if (response.success && response.data != null) {
      final exams = (response.data as List)
          .map((e) => ExamModel.fromJson(e))
          .toList();
      emit(ExamsLoaded(exams: exams));
    } else {
      emit(ExamError(message: response.message ?? 'Failed to load exams'));
    }
  }

  Future<void> _onLoadExamDetail(
      LoadExamDetail event, Emitter<ExamState> emit) async {
    emit(ExamLoading());

    final response = await apiService.get(
      '${AppConfig.examDetail}/${event.examId}',
      queryParams: {'language': event.language},
    );

    if (response.success && response.data != null) {
      final exam = ExamDetailModel.fromJson(response.data);
      emit(ExamDetailLoaded(exam: exam));
    } else {
      emit(ExamError(message: response.message ?? 'Failed to load exam details'));
    }
  }

  Future<void> _onStartExam(StartExam event, Emitter<ExamState> emit) async {
    emit(ExamLoading());

    final response = await apiService.post(
      AppConfig.examStart,
      body: {
        'examId': event.examId,
        'selectedLanguage': event.selectedLanguage,
        if (event.guestEmail != null) 'guestEmail': event.guestEmail,
      },
      includeAuth: true,
    );

    if (response.success && response.data != null) {
      final attemptId = response.data['attemptId'];
      final startedAt = DateTime.parse(response.data['startedAt']);
      emit(ExamStarted(attemptId: attemptId, startedAt: startedAt));
    } else {
      emit(ExamError(message: response.message ?? 'Failed to start exam'));
    }
  }

  Future<void> _onLoadExamQuestions(
      LoadExamQuestions event, Emitter<ExamState> emit) async {
    emit(ExamLoading());

    final response = await apiService.get(
      '${AppConfig.examQuestions}/${event.attemptId}',
      includeAuth: true,
    );

    if (response.success && response.data != null) {
      final questions = (response.data as List)
          .map((e) => QuestionModel.fromJson(e))
          .toList();
      emit(ExamQuestionsLoaded(
        questions: questions,
        attemptId: event.attemptId,
      ));
    } else {
      emit(ExamError(message: response.message ?? 'Failed to load questions'));
    }
  }

  Future<void> _onSubmitAnswer(
      SubmitAnswer event, Emitter<ExamState> emit) async {
    final response = await apiService.post(
      AppConfig.examSubmitAnswer,
      body: {
        'attemptId': event.attemptId,
        'questionId': event.questionId,
        'selectedOptionId': event.selectedOptionId,
        'timeSpentSeconds': event.timeSpentSeconds,
        'isMarkedForReview': event.isMarkedForReview,
      },
      includeAuth: true,
    );

    if (response.success) {
      emit(AnswerSubmitted(
          message: response.data?['message'] ?? 'Answer saved'));
    } else {
      emit(ExamError(message: response.message ?? 'Failed to submit answer'));
    }
  }

  Future<void> _onSubmitExam(SubmitExam event, Emitter<ExamState> emit) async {
    emit(ExamLoading());

    final response = await apiService.post(
      AppConfig.examSubmit,
      body: {'attemptId': event.attemptId},
      includeAuth: true,
    );

    if (response.success && response.data != null) {
      final result = ExamResultModel.fromJson(response.data);
      emit(ExamSubmitted(result: result));
    } else {
      emit(ExamError(message: response.message ?? 'Failed to submit exam'));
    }
  }

  Future<void> _onLoadExamResult(
      LoadExamResult event, Emitter<ExamState> emit) async {
    emit(ExamLoading());

    final response = await apiService.get(
      '${AppConfig.examResult}/${event.attemptId}',
      includeAuth: true,
    );

    if (response.success && response.data != null) {
      final result = ExamResultModel.fromJson(response.data);
      emit(ExamResultLoaded(result: result));
    } else {
      emit(ExamError(message: response.message ?? 'Failed to load result'));
    }
  }

  Future<void> _onLoadQuestionReview(
      LoadQuestionReview event, Emitter<ExamState> emit) async {
    emit(ExamLoading());

    final response = await apiService.get(
      '${AppConfig.examReview}/${event.attemptId}',
      includeAuth: true,
    );

    if (response.success && response.data != null) {
      final questions = (response.data as List)
          .map((e) => QuestionReviewModel.fromJson(e))
          .toList();
      emit(QuestionReviewLoaded(questions: questions));
    } else {
      emit(ExamError(
          message: response.message ?? 'Failed to load question review'));
    }
  }
}
