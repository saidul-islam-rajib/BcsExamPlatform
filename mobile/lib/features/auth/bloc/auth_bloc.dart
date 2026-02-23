export 'auth_bloc.dart';
export 'auth_event.dart';
export 'auth_state.dart';

import 'package:flutter_bloc/flutter_bloc.dart';
import '../../../core/config/app_config.dart';
import '../../../core/models/user_model.dart';
import '../../../core/services/api_service.dart';
import '../../../core/services/storage_service.dart';
import 'auth_event.dart';
import 'auth_state.dart';

class AuthBloc extends Bloc<AuthEvent, AuthState> {
  final ApiService apiService;

  AuthBloc({required this.apiService}) : super(AuthInitial()) {
    on<CheckAuthStatus>(_onCheckAuthStatus);
    on<LoginRequested>(_onLoginRequested);
    on<RegisterRequested>(_onRegisterRequested);
    on<LogoutRequested>(_onLogoutRequested);
    on<ContinueAsGuest>(_onContinueAsGuest);
  }

  Future<void> _onCheckAuthStatus(CheckAuthStatus event, Emitter<AuthState> emit) async {
    final token = StorageService.getString(AppConfig.keyAuthToken);
    final isGuest = StorageService.getBool(AppConfig.keyIsGuest) ?? false;

    if (isGuest) {
      emit(AuthGuest());
    } else if (token != null) {
      final userId = StorageService.getString(AppConfig.keyUserId) ?? '';
      final email = StorageService.getString(AppConfig.keyUserEmail) ?? '';
      final fullName = StorageService.getString(AppConfig.keyUserName) ?? '';
      final preferredLanguage = StorageService.getString(AppConfig.keyPreferredLanguage) ?? 'Bangla';

      final user = UserModel(
        userId: userId,
        email: email,
        fullName: fullName,
        isGuest: false,
        preferredLanguage: preferredLanguage,
      );

      emit(AuthAuthenticated(user: user, token: token));
    } else {
      emit(AuthUnauthenticated());
    }
  }

  Future<void> _onLoginRequested(LoginRequested event, Emitter<AuthState> emit) async {
    emit(AuthLoading());

    final response = await apiService.post(
      AppConfig.authLogin,
      body: {
        'email': event.email,
        'password': event.password,
      },
    );

    if (response.success && response.data != null) {
      final token = response.data['token'];
      final userData = response.data['user'];

      if (token != null && userData != null) {
        final user = UserModel.fromJson(userData);

        // Save to storage
        await StorageService.setString(AppConfig.keyAuthToken, token);
        await StorageService.setString(AppConfig.keyUserId, user.userId);
        await StorageService.setString(AppConfig.keyUserEmail, user.email);
        await StorageService.setString(AppConfig.keyUserName, user.fullName);
        await StorageService.setString(AppConfig.keyPreferredLanguage, user.preferredLanguage);
        await StorageService.setBool(AppConfig.keyIsGuest, false);

        emit(AuthAuthenticated(user: user, token: token));
      } else {
        emit(const AuthError(message: 'Invalid response from server'));
      }
    } else {
      emit(AuthError(message: response.message ?? 'Login failed'));
    }
  }

  Future<void> _onRegisterRequested(RegisterRequested event, Emitter<AuthState> emit) async {
    emit(AuthLoading());

    final response = await apiService.post(
      AppConfig.authRegister,
      body: {
        'email': event.email,
        'password': event.password,
        'fullName': event.fullName,
        'phoneNumber': event.phoneNumber,
        'preferredLanguage': event.preferredLanguage,
      },
    );

    if (response.success && response.data != null) {
      final token = response.data['token'];
      final userData = response.data['user'];

      if (token != null && userData != null) {
        final user = UserModel.fromJson(userData);

        // Save to storage
        await StorageService.setString(AppConfig.keyAuthToken, token);
        await StorageService.setString(AppConfig.keyUserId, user.userId);
        await StorageService.setString(AppConfig.keyUserEmail, user.email);
        await StorageService.setString(AppConfig.keyUserName, user.fullName);
        await StorageService.setString(AppConfig.keyPreferredLanguage, user.preferredLanguage);
        await StorageService.setBool(AppConfig.keyIsGuest, false);

        emit(AuthAuthenticated(user: user, token: token));
      } else {
        emit(const AuthError(message: 'Invalid response from server'));
      }
    } else {
      emit(AuthError(message: response.message ?? 'Registration failed'));
    }
  }

  Future<void> _onLogoutRequested(LogoutRequested event, Emitter<AuthState> emit) async {
    await StorageService.clear();
    emit(AuthUnauthenticated());
  }

  Future<void> _onContinueAsGuest(ContinueAsGuest event, Emitter<AuthState> emit) async {
    await StorageService.setBool(AppConfig.keyIsGuest, true);
    emit(AuthGuest());
  }
}
