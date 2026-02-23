import 'dart:convert';
import 'package:http/http.dart' as http;
import '../config/app_config.dart';
import 'storage_service.dart';

class ApiService {
  final String baseUrl = AppConfig.apiBaseUrl;

  Map<String, String> _getHeaders({bool includeAuth = false}) {
    final headers = {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
    };

    if (includeAuth) {
      final token = StorageService.getString(AppConfig.keyAuthToken);
      if (token != null) {
        headers['Authorization'] = 'Bearer $token';
      }
    }

    return headers;
  }

  Future<ApiResponse> get(String endpoint, {bool includeAuth = false, Map<String, String>? queryParams}) async {
    try {
      var uri = Uri.parse('$baseUrl$endpoint');
      if (queryParams != null) {
        uri = uri.replace(queryParameters: queryParams);
      }

      final response = await http.get(
        uri,
        headers: _getHeaders(includeAuth: includeAuth),
      ).timeout(Duration(milliseconds: AppConfig.apiTimeout));

      return _handleResponse(response);
    } catch (e) {
      return ApiResponse(
        success: false,
        message: 'Network error: ${e.toString()}',
      );
    }
  }

  Future<ApiResponse> post(String endpoint, {required Map<String, dynamic> body, bool includeAuth = false}) async {
    try {
      final response = await http.post(
        Uri.parse('$baseUrl$endpoint'),
        headers: _getHeaders(includeAuth: includeAuth),
        body: jsonEncode(body),
      ).timeout(Duration(milliseconds: AppConfig.apiTimeout));

      return _handleResponse(response);
    } catch (e) {
      return ApiResponse(
        success: false,
        message: 'Network error: ${e.toString()}',
      );
    }
  }

  Future<ApiResponse> put(String endpoint, {required Map<String, dynamic> body, bool includeAuth = false}) async {
    try {
      final response = await http.put(
        Uri.parse('$baseUrl$endpoint'),
        headers: _getHeaders(includeAuth: includeAuth),
        body: jsonEncode(body),
      ).timeout(Duration(milliseconds: AppConfig.apiTimeout));

      return _handleResponse(response);
    } catch (e) {
      return ApiResponse(
        success: false,
        message: 'Network error: ${e.toString()}',
      );
    }
  }

  Future<ApiResponse> delete(String endpoint, {bool includeAuth = false}) async {
    try {
      final response = await http.delete(
        Uri.parse('$baseUrl$endpoint'),
        headers: _getHeaders(includeAuth: includeAuth),
      ).timeout(Duration(milliseconds: AppConfig.apiTimeout));

      return _handleResponse(response);
    } catch (e) {
      return ApiResponse(
        success: false,
        message: 'Network error: ${e.toString()}',
      );
    }
  }

  ApiResponse _handleResponse(http.Response response) {
    try {
      final data = jsonDecode(response.body);

      if (response.statusCode >= 200 && response.statusCode < 300) {
        return ApiResponse(
          success: true,
          data: data,
          statusCode: response.statusCode,
        );
      } else {
        return ApiResponse(
          success: false,
          message: data['message'] ?? 'Request failed',
          statusCode: response.statusCode,
          data: data,
        );
      }
    } catch (e) {
      return ApiResponse(
        success: false,
        message: 'Failed to parse response: ${e.toString()}',
        statusCode: response.statusCode,
      );
    }
  }
}

class ApiResponse {
  final bool success;
  final String? message;
  final dynamic data;
  final int? statusCode;

  ApiResponse({
    required this.success,
    this.message,
    this.data,
    this.statusCode,
  });
}
