class QuestionModel {
  final String questionId;
  final int questionNumber;
  final String questionText;
  final String? questionImageUrl;
  final bool hasMathContent;
  final List<QuestionOption> options;
  final String subjectName;
  final String topicName;
  final String difficultyLevel;
  final double marks;

  QuestionModel({
    required this.questionId,
    required this.questionNumber,
    required this.questionText,
    this.questionImageUrl,
    required this.hasMathContent,
    required this.options,
    required this.subjectName,
    required this.topicName,
    required this.difficultyLevel,
    required this.marks,
  });

  factory QuestionModel.fromJson(Map<String, dynamic> json) {
    return QuestionModel(
      questionId: json['questionId'] ?? '',
      questionNumber: json['questionNumber'] ?? 0,
      questionText: json['questionText'] ?? '',
      questionImageUrl: json['questionImageUrl'],
      hasMathContent: json['hasMathContent'] ?? false,
      options: (json['options'] as List?)
              ?.map((e) => QuestionOption.fromJson(e))
              .toList() ??
          [],
      subjectName: json['subjectName'] ?? '',
      topicName: json['topicName'] ?? '',
      difficultyLevel: json['difficultyLevel'] ?? 'Easy',
      marks: (json['marks'] ?? 1.0).toDouble(),
    );
  }
}

class QuestionOption {
  final String optionId;
  final String optionText;
  final int optionOrder;

  QuestionOption({
    required this.optionId,
    required this.optionText,
    required this.optionOrder,
  });

  factory QuestionOption.fromJson(Map<String, dynamic> json) {
    return QuestionOption(
      optionId: json['optionId'] ?? '',
      optionText: json['optionText'] ?? '',
      optionOrder: json['optionOrder'] ?? 0,
    );
  }
}

class QuestionReviewModel {
  final String questionId;
  final int questionNumber;
  final String questionText;
  final String? questionImageUrl;
  final List<QuestionOptionReview> options;
  final String? userSelectedOptionId;
  final String correctOptionId;
  final bool isCorrect;
  final double marksObtained;
  final int timeSpentSeconds;
  final String explanation;
  final String subjectName;
  final String topicName;
  final String difficultyLevel;
  final String? sourceReference;

  QuestionReviewModel({
    required this.questionId,
    required this.questionNumber,
    required this.questionText,
    this.questionImageUrl,
    required this.options,
    this.userSelectedOptionId,
    required this.correctOptionId,
    required this.isCorrect,
    required this.marksObtained,
    required this.timeSpentSeconds,
    required this.explanation,
    required this.subjectName,
    required this.topicName,
    required this.difficultyLevel,
    this.sourceReference,
  });

  factory QuestionReviewModel.fromJson(Map<String, dynamic> json) {
    return QuestionReviewModel(
      questionId: json['questionId'] ?? '',
      questionNumber: json['questionNumber'] ?? 0,
      questionText: json['questionText'] ?? '',
      questionImageUrl: json['questionImageUrl'],
      options: (json['options'] as List?)
              ?.map((e) => QuestionOptionReview.fromJson(e))
              .toList() ??
          [],
      userSelectedOptionId: json['userSelectedOptionId'],
      correctOptionId: json['correctOptionId'] ?? '',
      isCorrect: json['isCorrect'] ?? false,
      marksObtained: (json['marksObtained'] ?? 0.0).toDouble(),
      timeSpentSeconds: json['timeSpentSeconds'] ?? 0,
      explanation: json['explanation'] ?? '',
      subjectName: json['subjectName'] ?? '',
      topicName: json['topicName'] ?? '',
      difficultyLevel: json['difficultyLevel'] ?? 'Easy',
      sourceReference: json['sourceReference'],
    );
  }
}

class QuestionOptionReview {
  final String optionId;
  final String optionText;
  final int optionOrder;
  final bool isCorrect;

  QuestionOptionReview({
    required this.optionId,
    required this.optionText,
    required this.optionOrder,
    required this.isCorrect,
  });

  factory QuestionOptionReview.fromJson(Map<String, dynamic> json) {
    return QuestionOptionReview(
      optionId: json['optionId'] ?? '',
      optionText: json['optionText'] ?? '',
      optionOrder: json['optionOrder'] ?? 0,
      isCorrect: json['isCorrect'] ?? false,
    );
  }
}
