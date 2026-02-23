// Alias for backward compatibility
typedef ResultModel = ExamResultModel;

class ExamResultModel {
  final String attemptId;
  final String examName;
  final double totalMarksObtained;
  final double totalMarks;
  final int totalCorrectAnswers;
  final int totalWrongAnswers;
  final int totalUnanswered;
  final double accuracyPercentage;
  final int totalTimeSpentSeconds;
  final int rank;
  final String badgeName;
  final String badgeColor;
  final bool isPassed;

  ExamResultModel({
    required this.attemptId,
    required this.examName,
    required this.totalMarksObtained,
    required this.totalMarks,
    required this.totalCorrectAnswers,
    required this.totalWrongAnswers,
    required this.totalUnanswered,
    required this.accuracyPercentage,
    required this.totalTimeSpentSeconds,
    required this.rank,
    required this.badgeName,
    required this.badgeColor,
    required this.isPassed,
  });

  factory ExamResultModel.fromJson(Map<String, dynamic> json) {
    return ExamResultModel(
      attemptId: json['attemptId'] ?? '',
      examName: json['examName'] ?? '',
      totalMarksObtained: (json['totalMarksObtained'] ?? 0.0).toDouble(),
      totalMarks: (json['totalMarks'] ?? 200.0).toDouble(),
      totalCorrectAnswers: json['totalCorrectAnswers'] ?? 0,
      totalWrongAnswers: json['totalWrongAnswers'] ?? 0,
      totalUnanswered: json['totalUnanswered'] ?? 0,
      accuracyPercentage: (json['accuracyPercentage'] ?? 0.0).toDouble(),
      totalTimeSpentSeconds: json['totalTimeSpentSeconds'] ?? 0,
      rank: json['rank'] ?? 0,
      badgeName: json['badgeName'] ?? 'N/A',
      badgeColor: json['badgeColor'] ?? '#000000',
      isPassed: json['isPassed'] ?? false,
    );
  }

  String get formattedTime {
    final hours = totalTimeSpentSeconds ~/ 3600;
    final minutes = (totalTimeSpentSeconds % 3600) ~/ 60;
    final seconds = totalTimeSpentSeconds % 60;
    
    if (hours > 0) {
      return '${hours}h ${minutes}m ${seconds}s';
    } else if (minutes > 0) {
      return '${minutes}m ${seconds}s';
    } else {
      return '${seconds}s';
    }
  }

  String get accuracyFormatted => '${accuracyPercentage.toStringAsFixed(1)}%';
}
