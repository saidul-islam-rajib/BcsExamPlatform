import 'package:flutter_dotenv/flutter_dotenv.dart';

class AppConfig {
  static String get appName => dotenv.env['APP_NAME'] ?? 'BCS Exam Platform';
  static String get apiBaseUrl => dotenv.env['API_BASE_URL'] ?? 'http://localhost:5000/api';
  static int get apiTimeout => int.parse(dotenv.env['API_TIMEOUT'] ?? '30000');
  static String get defaultLanguage => dotenv.env['DEFAULT_LANGUAGE'] ?? 'Bangla';

  // API Endpoints
  static const String authRegister = '/auth/register';
  static const String authLogin = '/auth/login';
  static const String examList = '/exam/list';
  static const String examDetail = '/exam';
  static const String examStart = '/exam/start';
  static const String examQuestions = '/exam/questions';
  static const String examSubmitAnswer = '/exam/submit-answer';
  static const String examSubmit = '/exam/submit';
  static const String examResult = '/exam/result';
  static const String examReview = '/exam/review';

  // Storage Keys
  static const String keyAuthToken = 'auth_token';
  static const String keyUserId = 'user_id';
  static const String keyUserEmail = 'user_email';
  static const String keyUserName = 'user_name';
  static const String keyPreferredLanguage = 'preferred_language';
  static const String keyIsGuest = 'is_guest';

  // App Constants
  static const int questionsPerPage = 20;
  static const int defaultExamDuration = 120; // minutes
  static const int defaultTotalQuestions = 200;
}
