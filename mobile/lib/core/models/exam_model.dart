class ExamModel {
  final String examId;
  final String examName;
  final DateTime examDate;
  final int totalQuestions;
  final int durationMinutes;
  final double totalMarks;
  final String languageMode;
  final bool isPaid;
  final double? examFee;
  final bool allowGuestUsers;
  final bool isPublished;

  ExamModel({
    required this.examId,
    required this.examName,
    required this.examDate,
    required this.totalQuestions,
    required this.durationMinutes,
    required this.totalMarks,
    required this.languageMode,
    required this.isPaid,
    this.examFee,
    required this.allowGuestUsers,
    required this.isPublished,
  });

  factory ExamModel.fromJson(Map<String, dynamic> json) {
    return ExamModel(
      examId: json['examId'] ?? '',
      examName: json['examName'] ?? '',
      examDate: DateTime.parse(json['examDate']),
      totalQuestions: json['totalQuestions'] ?? 200,
      durationMinutes: json['durationMinutes'] ?? 120,
      totalMarks: (json['totalMarks'] ?? 200.0).toDouble(),
      languageMode: json['languageMode'] ?? 'Bilingual',
      isPaid: json['isPaid'] ?? false,
      examFee: json['examFee']?.toDouble(),
      allowGuestUsers: json['allowGuestUsers'] ?? true,
      isPublished: json['isPublished'] ?? false,
    );
  }
}

class ExamDetailModel extends ExamModel {
  final List<SubjectDistribution> subjectDistributions;

  ExamDetailModel({
    required super.examId,
    required super.examName,
    required super.examDate,
    required super.totalQuestions,
    required super.durationMinutes,
    required super.totalMarks,
    required super.languageMode,
    required super.isPaid,
    super.examFee,
    required super.allowGuestUsers,
    required super.isPublished,
    required this.subjectDistributions,
  });

  factory ExamDetailModel.fromJson(Map<String, dynamic> json) {
    return ExamDetailModel(
      examId: json['examId'] ?? '',
      examName: json['examName'] ?? '',
      examDate: DateTime.parse(json['examDate']),
      totalQuestions: json['totalQuestions'] ?? 200,
      durationMinutes: json['durationMinutes'] ?? 120,
      totalMarks: (json['totalMarks'] ?? 200.0).toDouble(),
      languageMode: json['languageMode'] ?? 'Bilingual',
      isPaid: json['isPaid'] ?? false,
      examFee: json['examFee']?.toDouble(),
      allowGuestUsers: json['allowGuestUsers'] ?? true,
      isPublished: json['isPublished'] ?? false,
      subjectDistributions: (json['subjectDistributions'] as List?)
              ?.map((e) => SubjectDistribution.fromJson(e))
              .toList() ??
          [],
    );
  }
}

class SubjectDistribution {
  final String subjectName;
  final int totalQuestions;
  final int easyQuestions;
  final int intermediateQuestions;
  final int hardQuestions;

  SubjectDistribution({
    required this.subjectName,
    required this.totalQuestions,
    required this.easyQuestions,
    required this.intermediateQuestions,
    required this.hardQuestions,
  });

  factory SubjectDistribution.fromJson(Map<String, dynamic> json) {
    return SubjectDistribution(
      subjectName: json['subjectName'] ?? '',
      totalQuestions: json['totalQuestions'] ?? 0,
      easyQuestions: json['easyQuestions'] ?? 0,
      intermediateQuestions: json['intermediateQuestions'] ?? 0,
      hardQuestions: json['hardQuestions'] ?? 0,
    );
  }
}
