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
      userId: (json['userId'] ?? json['UserId'] ?? '').toString(),
      email: json['email'] ?? json['Email'] ?? '',
      fullName: json['fullName'] ?? json['FullName'] ?? '',
      phoneNumber: json['phoneNumber'] ?? json['PhoneNumber'],
      isGuest: json['isGuest'] ?? json['IsGuest'] ?? false,
      preferredLanguage: json['preferredLanguage'] ?? json['PreferredLanguage'] ?? 'Bangla',
      profileImageUrl: json['profileImageUrl'] ?? json['ProfileImageUrl'],
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
