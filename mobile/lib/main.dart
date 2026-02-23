import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:hive_flutter/hive_flutter.dart';

import 'core/config/app_config.dart';
import 'core/config/theme_config.dart';
import 'core/routes/app_router.dart';
import 'core/services/api_service.dart';
import 'core/services/storage_service.dart';
import 'features/auth/bloc/auth_bloc.dart';
import 'features/exam/bloc/exam_bloc.dart';

void main() async {
  WidgetsFlutterBinding.ensureInitialized();

  // Load environment variables
  await dotenv.load(fileName: ".env");

  // Initialize Hive
  await Hive.initFlutter();
  await StorageService.init();

  // Initialize services
  final apiService = ApiService();

  // Set preferred orientations
  await SystemChrome.setPreferredOrientations([
    DeviceOrientation.portraitUp,
    DeviceOrientation.portraitDown,
  ]);

  runApp(MyApp(apiService: apiService));
}

class MyApp extends StatelessWidget {
  final ApiService apiService;

  const MyApp({super.key, required this.apiService});

  @override
  Widget build(BuildContext context) {
    return MultiBlocProvider(
      providers: [
        BlocProvider(
          create: (context) => AuthBloc(apiService: apiService)..add(CheckAuthStatus()),
        ),
        BlocProvider(
          create: (context) => ExamBloc(apiService: apiService),
        ),
      ],
      child: MaterialApp.router(
        title: AppConfig.appName,
        debugShowCheckedModeBanner: false,
        theme: ThemeConfig.lightTheme,
        routerConfig: AppRouter.router,
      ),
    );
  }
}
