import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import '../bloc/exam_bloc.dart';

class ReviewScreen extends StatefulWidget {
  final String attemptId;

  const ReviewScreen({
    super.key,
    required this.attemptId,
  });

  @override
  State<ReviewScreen> createState() => _ReviewScreenState();
}

class _ReviewScreenState extends State<ReviewScreen> {
  String _filter = 'All';

  @override
  void initState() {
    super.initState();
    context.read<ExamBloc>().add(
          LoadReviewRequested(attemptId: widget.attemptId),
        );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Review Answers'),
        actions: [
          PopupMenuButton<String>(
            initialValue: _filter,
            onSelected: (value) {
              setState(() {
                _filter = value;
              });
            },
            itemBuilder: (context) => [
              const PopupMenuItem(value: 'All', child: Text('All Questions')),
              const PopupMenuItem(value: 'Correct', child: Text('Correct Only')),
              const PopupMenuItem(value: 'Wrong', child: Text('Wrong Only')),
              const PopupMenuItem(value: 'Unanswered', child: Text('Unanswered Only')),
            ],
          ),
        ],
      ),
      body: BlocBuilder<ExamBloc, ExamState>(
        builder: (context, state) {
          if (state is ExamLoading) {
            return const Center(child: CircularProgressIndicator());
          }

          if (state is ExamFailure) {
            return Center(
              child: Column(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  const Icon(Icons.error_outline, size: 64, color: Colors.red),
                  const SizedBox(height: 16),
                  Text(state.error, style: const TextStyle(color: Colors.red)),
                  const SizedBox(height: 16),
                  ElevatedButton(
                    onPressed: () {
                      context.read<ExamBloc>().add(
                            LoadReviewRequested(attemptId: widget.attemptId),
                          );
                    },
                    child: const Text('Retry'),
                  ),
                ],
              ),
            );
          }

          if (state is ReviewLoaded) {
            var filteredAnswers = state.answers;
            
            if (_filter == 'Correct') {
              filteredAnswers = filteredAnswers.where((a) => a.isCorrect).toList();
            } else if (_filter == 'Wrong') {
              filteredAnswers = filteredAnswers.where((a) => !a.isCorrect && a.selectedOptionId != null).toList();
            } else if (_filter == 'Unanswered') {
              filteredAnswers = filteredAnswers.where((a) => a.selectedOptionId == null).toList();
            }

            return ListView.builder(
              padding: const EdgeInsets.all(16),
              itemCount: filteredAnswers.length,
              itemBuilder: (context, index) {
                final answer = filteredAnswers[index];
                final question = answer.question;
                
                return Card(
                  margin: const EdgeInsets.only(bottom: 16),
                  child: Padding(
                    padding: const EdgeInsets.all(16.0),
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        // Question Header
                        Row(
                          children: [
                            Container(
                              padding: const EdgeInsets.symmetric(
                                horizontal: 12,
                                vertical: 6,
                              ),
                              decoration: BoxDecoration(
                                color: answer.isCorrect
                                    ? Colors.green
                                    : answer.selectedOptionId == null
                                        ? Colors.grey
                                        : Colors.red,
                                borderRadius: BorderRadius.circular(12),
                              ),
                              child: Text(
                                answer.isCorrect
                                    ? 'Correct'
                                    : answer.selectedOptionId == null
                                        ? 'Unanswered'
                                        : 'Wrong',
                                style: const TextStyle(
                                  color: Colors.white,
                                  fontSize: 12,
                                  fontWeight: FontWeight.bold,
                                ),
                              ),
                            ),
                            const Spacer(),
                            Text(
                              'Q${index + 1}',
                              style: const TextStyle(
                                fontSize: 16,
                                fontWeight: FontWeight.bold,
                                color: Colors.blue,
                              ),
                            ),
                          ],
                        ),
                        const SizedBox(height: 12),

                        // Question Text
                        Text(
                          question.questionTextEnglish,
                          style: const TextStyle(
                            fontSize: 16,
                            fontWeight: FontWeight.w500,
                          ),
                        ),
                        if (question.questionTextBangla.isNotEmpty) ...[
                          const SizedBox(height: 8),
                          Text(
                            question.questionTextBangla,
                            style: const TextStyle(
                              fontSize: 14,
                              color: Colors.grey,
                            ),
                          ),
                        ],
                        const SizedBox(height: 16),

                        // Options
                        ...question.options.asMap().entries.map((entry) {
                          final optIndex = entry.key;
                          final option = entry.value;
                          final optionLabel = String.fromCharCode(65 + optIndex);
                          final isUserAnswer = answer.selectedOptionId == option.optionId;
                          final isCorrectAnswer = option.isCorrect;

                          Color? backgroundColor;
                          if (isCorrectAnswer) {
                            backgroundColor = Colors.green.withOpacity(0.1);
                          } else if (isUserAnswer && !isCorrectAnswer) {
                            backgroundColor = Colors.red.withOpacity(0.1);
                          }

                          return Container(
                            margin: const EdgeInsets.only(bottom: 8),
                            padding: const EdgeInsets.all(12),
                            decoration: BoxDecoration(
                              color: backgroundColor,
                              border: Border.all(
                                color: isCorrectAnswer
                                    ? Colors.green
                                    : isUserAnswer
                                        ? Colors.red
                                        : Colors.grey.shade300,
                                width: isCorrectAnswer || isUserAnswer ? 2 : 1,
                              ),
                              borderRadius: BorderRadius.circular(8),
                            ),
                            child: Row(
                              children: [
                                Text(
                                  '$optionLabel.',
                                  style: TextStyle(
                                    fontWeight: FontWeight.bold,
                                    color: isCorrectAnswer
                                        ? Colors.green
                                        : isUserAnswer
                                            ? Colors.red
                                            : Colors.black,
                                  ),
                                ),
                                const SizedBox(width: 8),
                                Expanded(
                                  child: Text(option.optionTextEnglish),
                                ),
                                if (isCorrectAnswer)
                                  const Icon(Icons.check_circle, color: Colors.green, size: 20),
                                if (isUserAnswer && !isCorrectAnswer)
                                  const Icon(Icons.cancel, color: Colors.red, size: 20),
                              ],
                            ),
                          );
                        }).toList(),

                        // Explanation
                        if (question.explanation != null) ...[
                          const SizedBox(height: 16),
                          Container(
                            padding: const EdgeInsets.all(12),
                            decoration: BoxDecoration(
                              color: Colors.blue.withOpacity(0.05),
                              borderRadius: BorderRadius.circular(8),
                              border: Border.all(color: Colors.blue.withOpacity(0.2)),
                            ),
                            child: Column(
                              crossAxisAlignment: CrossAxisAlignment.start,
                              children: [
                                const Row(
                                  children: [
                                    Icon(Icons.lightbulb, size: 16, color: Colors.blue),
                                    SizedBox(width: 4),
                                    Text(
                                      'Explanation',
                                      style: TextStyle(
                                        fontWeight: FontWeight.bold,
                                        color: Colors.blue,
                                      ),
                                    ),
                                  ],
                                ),
                                const SizedBox(height: 8),
                                Text(question.explanation!.explanationEnglish),
                                if (question.explanation!.explanationBangla.isNotEmpty) ...[
                                  const SizedBox(height: 8),
                                  Text(
                                    question.explanation!.explanationBangla,
                                    style: const TextStyle(color: Colors.grey),
                                  ),
                                ],
                              ],
                            ),
                          ),
                        ],

                        // Metadata
                        const SizedBox(height: 12),
                        Wrap(
                          spacing: 8,
                          runSpacing: 8,
                          children: [
                            Chip(
                              label: Text(question.subjectNameEnglish),
                              backgroundColor: Colors.blue.withOpacity(0.1),
                              labelStyle: const TextStyle(fontSize: 12),
                            ),
                            if (question.topicNameEnglish.isNotEmpty)
                              Chip(
                                label: Text(question.topicNameEnglish),
                                backgroundColor: Colors.orange.withOpacity(0.1),
                                labelStyle: const TextStyle(fontSize: 12),
                              ),
                            Chip(
                              label: Text(question.difficultyLevel),
                              backgroundColor: Colors.purple.withOpacity(0.1),
                              labelStyle: const TextStyle(fontSize: 12),
                            ),
                          ],
                        ),
                      ],
                    ),
                  ),
                );
              },
            );
          }

          return const SizedBox.shrink();
        },
      ),
    );
  }
}
