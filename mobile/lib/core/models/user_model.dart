class UserModel {
  final String userId;
  final String email;
  final String fullName;
  final String? phoneNumber;
  final bool isGuest;
  final String preferredLanguage;
  final String? profileImageUrl;

  UserModel({
    required this.userId,
    required this.email,
    required this.fullName,
    this.phoneNumber,
    required this.isGuest,
    required this.preferredLanguage,
    this.profileImageUrl,
  });

  factory UserModel.fromJson(Map<String, dynamic> json) {
    return UserModel(
      userId: json['userId'] ?? '',
      email: json['email'] ?? '',
      fullName: json['fullName'] ?? '',
      phoneNumber: json['phoneNumber'],
      isGuest: json['isGuest'] ?? false,
      preferredLanguage: json['preferredLanguage'] ?? 'Bangla',
      profileImageUrl: json['profileImageUrl'],
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'userId': userId,
      'email': email,
      'fullName': fullName,
      'phoneNumber': phoneNumber,
      'isGuest': isGuest,
      'preferredLanguage': preferredLanguage,
      'profileImageUrl': profileImageUrl,
    };
  }
}
