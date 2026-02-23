import 'package:equatable/equatable.dart';
import '../../../core/models/user_model.dart';

abstract class AuthState extends Equatable {
  const AuthState();

  @override
  List<Object?> get props => [];
}

class AuthInitial extends AuthState {}

class AuthLoading extends AuthState {}

class AuthSuccess extends AuthState {
  final UserModel user;
  final String token;

  const AuthSuccess({
    required this.user,
    required this.token,
  });

  @override
  List<Object?> get props => [user, token];
}

class AuthAuthenticated extends AuthState {
  final UserModel user;
  final String token;

  const AuthAuthenticated({
    required this.user,
    required this.token,
  });

  @override
  List<Object?> get props => [user, token];
}

class AuthGuest extends AuthState {}

class AuthUnauthenticated extends AuthState {}

class AuthFailure extends AuthState {
  final String error;

  const AuthFailure({required this.error});

  @override
  List<Object?> get props => [error];
}

class AuthError extends AuthState {
  final String message;

  const AuthError({required this.message});

  @override
  List<Object?> get props => [message];
}
