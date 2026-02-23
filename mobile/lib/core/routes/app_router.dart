import 'package:go_router/go_router.dart';
import '../../features/auth/screens/login_screen.dart';
import '../../features/auth/screens/register_screen.dart';
import '../../features/auth/screens/splash_screen.dart';
import '../../features/home/screens/home_screen.dart';
import '../../features/exam/screens/exam_list_screen.dart';
import '../../features/exam/screens/exam_details_screen.dart';
import '../../features/exam/screens/exam_screen.dart';
import '../../features/exam/screens/result_screen.dart';
import '../../features/exam/screens/review_screen.dart';
import '../../features/profile/screens/profile_screen.dart';
import '../../features/settings/screens/settings_screen.dart';

class AppRouter {
  static final GoRouter router = GoRouter(
    initialLocation: '/',
    routes: [
      GoRoute(
        path: '/',
        builder: (context, state) => const SplashScreen(),
      ),
      GoRoute(
        path: '/login',
        builder: (context, state) => const LoginScreen(),
      ),
      GoRoute(
        path: '/register',
        builder: (context, state) => const RegisterScreen(),
      ),
      GoRoute(
        path: '/home',
        builder: (context, state) => const HomeScreen(),
      ),
      GoRoute(
        path: '/exams',
        builder: (context, state) => const ExamListScreen(),
      ),
      GoRoute(
        path: '/exam-details/:examId',
        builder: (context, state) {
          final examId = state.pathParameters['examId']!;
          return ExamDetailsScreen(examId: examId);
        },
      ),
      GoRoute(
        path: '/exam/:attemptId',
        builder: (context, state) {
          final attemptId = state.pathParameters['attemptId']!;
          return ExamScreen(attemptId: attemptId);
        },
      ),
      GoRoute(
        path: '/result/:attemptId',
        builder: (context, state) {
          final attemptId = state.pathParameters['attemptId']!;
          return ResultScreen(attemptId: attemptId);
        },
      ),
      GoRoute(
        path: '/review/:attemptId',
        builder: (context, state) {
          final attemptId = state.pathParameters['attemptId']!;
          return ReviewScreen(attemptId: attemptId);
        },
      ),
      GoRoute(
        path: '/profile',
        builder: (context, state) => const ProfileScreen(),
      ),
      GoRoute(
        path: '/settings',
        builder: (context, state) => const SettingsScreen(),
      ),
    ],
  );
}
