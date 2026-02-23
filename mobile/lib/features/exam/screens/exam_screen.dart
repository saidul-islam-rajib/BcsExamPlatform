import 'dart:async';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:go_router/go_router.dart';
import '../bloc/exam_bloc.dart';
import '../../../core/models/question_model.dart';

class ExamScreen extends StatefulWidget {
  final String attemptId;

  const ExamScreen({
    super.key,
    required this.attemptId,
  });

  @override
  State<ExamScreen> createState() => _ExamScreenState();
}

class _ExamScreenState extends State<ExamScreen> {
  int _currentQuestionIndex = 0;
  Timer? _timer;
  int _remainingSeconds = 0;
  final Map<String, String?> _answers = {};
  final Set<int> _markedForReview = {};
  bool _showQuestionPalette = false;

  @override
  void initState() {
    super.initState();
    context.read<ExamBloc>().add(
          LoadExamQuestionsRequested(attemptId: widget.attemptId),
        );
  }

  @override
  void dispose() {
    _timer?.cancel();
    super.dispose();
  }

  void _startTimer(int durationMinutes) {
    _remainingSeconds = durationMinutes * 60;
    _timer = Timer.periodic(const Duration(seconds: 1), (timer) {
      if (_remainingSeconds > 0) {
        setState(() {
          _remainingSeconds--;
        });
      } else {
        _timer?.cancel();
        _submitExam();
      }
    });
  }

  String _formatTime(int seconds) {
    final hours = seconds ~/ 3600;
    final minutes = (seconds % 3600) ~/ 60;
    final secs = seconds % 60;
    return '${hours.toString().padLeft(2, '0')}:${minutes.toString().padLeft(2, '0')}:${secs.toString().padLeft(2, '0')}';
  }

  void _selectAnswer(String questionId, String optionId) {
    setState(() {
      _answers[questionId] = optionId;
    });
    
    // Auto-save answer
    context.read<ExamBloc>().add(
          SubmitAnswerRequested(
            attemptId: widget.attemptId,
            questionId: questionId,
            selectedOptionId: optionId,
          ),
        );
  }

  void _toggleMarkForReview() {
    setState(() {
      if (_markedForReview.contains(_currentQuestionIndex)) {
        _markedForReview.remove(_currentQuestionIndex);
      } else {
        _markedForReview.add(_currentQuestionIndex);
      }
    });
  }

  void _goToQuestion(int index) {
    setState(() {
      _currentQuestionIndex = index;
      _showQuestionPalette = false;
    });
  }

  void _nextQuestion(int totalQuestions) {
    if (_currentQuestionIndex < totalQuestions - 1) {
      setState(() {
        _currentQuestionIndex++;
      });
    }
  }

  void _previousQuestion() {
    if (_currentQuestionIndex > 0) {
      setState(() {
        _currentQuestionIndex--;
      });
    }
  }

  void _submitExam() {
    showDialog(
      context: context,
      barrierDismissible: false,
      builder: (context) => AlertDialog(
        title: const Text('Submit Exam'),
        content: const Text(
          'Are you sure you want to submit the exam? You cannot change your answers after submission.',
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context),
            child: const Text('Cancel'),
          ),
          ElevatedButton(
            onPressed: () {
              Navigator.pop(context);
              _timer?.cancel();
              context.read<ExamBloc>().add(
                    SubmitExamRequested(attemptId: widget.attemptId),
                  );
            },
            child: const Text('Submit'),
          ),
        ],
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return WillPopScope(
      onWillPop: () async {
        final shouldPop = await showDialog<bool>(
          context: context,
          builder: (context) => AlertDialog(
            title: const Text('Exit Exam'),
            content: const Text(
              'Are you sure you want to exit? Your progress will be saved but the timer will continue.',
            ),
            actions: [
              TextButton(
                onPressed: () => Navigator.pop(context, false),
                child: const Text('Cancel'),
              ),
              ElevatedButton(
                onPressed: () => Navigator.pop(context, true),
                child: const Text('Exit'),
              ),
            ],
          ),
        );
        return shouldPop ?? false;
      },
      child: Scaffold(
        appBar: AppBar(
          title: Text(_formatTime(_remainingSeconds)),
          centerTitle: true,
          actions: [
            IconButton(
              icon: const Icon(Icons.grid_view),
              onPressed: () {
                setState(() {
                  _showQuestionPalette = !_showQuestionPalette;
                });
              },
            ),
          ],
        ),
        body: BlocConsumer<ExamBloc, ExamState>(
          listener: (context, state) {
            if (state is ExamSubmitted) {
              context.go('/result/${widget.attemptId}');
            } else if (state is ExamFailure) {
              ScaffoldMessenger.of(context).showSnackBar(
                SnackBar(
                  content: Text(state.error),
                  backgroundColor: Colors.red,
                ),
              );
            }
          },
          builder: (context, state) {
            if (state is ExamLoading) {
              return const Center(
                child: CircularProgressIndicator(),
              );
            }

            if (state is ExamQuestionsLoaded) {
              if (_timer == null) {
                _startTimer(state.durationMinutes);
              }

              final questions = state.questions;
              if (questions.isEmpty) {
                return const Center(
                  child: Text('No questions available'),
                );
              }

              final currentQuestion = questions[_currentQuestionIndex];

              return Column(
                children: [
                  // Progress Bar
                  LinearProgressIndicator(
                    value: (_currentQuestionIndex + 1) / questions.length,
                    backgroundColor: Colors.grey[200],
                  ),

                  // Question Palette Overlay
                  if (_showQuestionPalette)
                    Expanded(
                      child: _buildQuestionPalette(questions),
                    )
                  else
                    Expanded(
                      child: SingleChildScrollView(
                        padding: const EdgeInsets.all(16.0),
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            // Question Number
                            Row(
                              mainAxisAlignment: MainAxisAlignment.spaceBetween,
                              children: [
                                Text(
                                  'Question ${_currentQuestionIndex + 1}/${questions.length}',
                                  style: const TextStyle(
                                    fontSize: 16,
                                    fontWeight: FontWeight.bold,
                                    color: Colors.blue,
                                  ),
                                ),
                                Row(
                                  children: [
                                    Text(
                                      '${currentQuestion.marks} Mark${currentQuestion.marks > 1 ? 's' : ''}',
                                      style: const TextStyle(
                                        fontSize: 14,
                                        color: Colors.grey,
                                      ),
                                    ),
                                    const SizedBox(width: 8),
                                    Chip(
                                      label: Text(
                                        currentQuestion.difficultyLevel,
                                        style: const TextStyle(fontSize: 12),
                                      ),
                                      backgroundColor: _getDifficultyColor(
                                        currentQuestion.difficultyLevel,
                                      ),
                                    ),
                                  ],
                                ),
                              ],
                            ),
                            const SizedBox(height: 16),

                            // Question Text
                            Card(
                              child: Padding(
                                padding: const EdgeInsets.all(16.0),
                                child: Column(
                                  crossAxisAlignment: CrossAxisAlignment.start,
                                  children: [
                                    Text(
                                      currentQuestion.questionTextEnglish,
                                      style: const TextStyle(
                                        fontSize: 16,
                                        height: 1.5,
                                      ),
                                    ),
                                    if (currentQuestion.questionTextBangla.isNotEmpty) ...[
                                      const SizedBox(height: 12),
                                      const Divider(),
                                      const SizedBox(height: 12),
                                      Text(
                                        currentQuestion.questionTextBangla,
                                        style: const TextStyle(
                                          fontSize: 16,
                                          height: 1.5,
                                        ),
                                      ),
                                    ],
                                  ],
                                ),
                              ),
                            ),
                            const SizedBox(height: 16),

                            // Options
                            ...currentQuestion.options.asMap().entries.map((entry) {
                              final index = entry.key;
                              final option = entry.value;
                              final optionLabel = String.fromCharCode(65 + index); // A, B, C, D
                              final isSelected = _answers[currentQuestion.questionId] == option.optionId;

                              return Card(
                                margin: const EdgeInsets.only(bottom: 12),
                                color: isSelected ? Colors.blue.withOpacity(0.1) : null,
                                child: InkWell(
                                  onTap: () => _selectAnswer(
                                    currentQuestion.questionId,
                                    option.optionId,
                                  ),
                                  borderRadius: BorderRadius.circular(12),
                                  child: Padding(
                                    padding: const EdgeInsets.all(16.0),
                                    child: Row(
                                      children: [
                                        Container(
                                          width: 32,
                                          height: 32,
                                          decoration: BoxDecoration(
                                            shape: BoxShape.circle,
                                            color: isSelected
                                                ? Colors.blue
                                                : Colors.grey[200],
                                          ),
                                          child: Center(
                                            child: Text(
                                              optionLabel,
                                              style: TextStyle(
                                                color: isSelected
                                                    ? Colors.white
                                                    : Colors.black,
                                                fontWeight: FontWeight.bold,
                                              ),
                                            ),
                                          ),
                                        ),
                                        const SizedBox(width: 12),
                                        Expanded(
                                          child: Column(
                                            crossAxisAlignment: CrossAxisAlignment.start,
                                            children: [
                                              Text(
                                                option.optionTextEnglish,
                                                style: const TextStyle(fontSize: 14),
                                              ),
                                              if (option.optionTextBangla.isNotEmpty) ...[
                                                const SizedBox(height: 4),
                                                Text(
                                                  option.optionTextBangla,
                                                  style: const TextStyle(
                                                    fontSize: 14,
                                                    color: Colors.grey,
                                                  ),
                                                ),
                                              ],
                                            ],
                                          ),
                                        ),
                                        if (isSelected)
                                          const Icon(
                                            Icons.check_circle,
                                            color: Colors.blue,
                                          ),
                                      ],
                                    ),
                                  ),
                                ),
                              );
                            }).toList(),

                            const SizedBox(height: 16),

                            // Mark for Review
                            CheckboxListTile(
                              title: const Text('Mark for Review'),
                              value: _markedForReview.contains(_currentQuestionIndex),
                              onChanged: (value) => _toggleMarkForReview(),
                              controlAffinity: ListTileControlAffinity.leading,
                            ),
                          ],
                        ),
                      ),
                    ),

                  // Navigation Buttons
                  Container(
                    padding: const EdgeInsets.all(16.0),
                    decoration: BoxDecoration(
                      color: Colors.white,
                      boxShadow: [
                        BoxShadow(
                          color: Colors.grey.withOpacity(0.2),
                          spreadRadius: 1,
                          blurRadius: 5,
                          offset: const Offset(0, -3),
                        ),
                      ],
                    ),
                    child: Row(
                      children: [
                        Expanded(
                          child: OutlinedButton(
                            onPressed: _currentQuestionIndex > 0
                                ? _previousQuestion
                                : null,
                            child: const Text('Previous'),
                          ),
                        ),
                        const SizedBox(width: 12),
                        Expanded(
                          child: ElevatedButton(
                            onPressed: _currentQuestionIndex < questions.length - 1
                                ? () => _nextQuestion(questions.length)
                                : _submitExam,
                            child: Text(
                              _currentQuestionIndex < questions.length - 1
                                  ? 'Next'
                                  : 'Submit',
                            ),
                          ),
                        ),
                      ],
                    ),
                  ),
                ],
              );
            }

            return const SizedBox.shrink();
          },
        ),
      ),
    );
  }

  Widget _buildQuestionPalette(List<QuestionModel> questions) {
    return Container(
      color: Colors.white,
      padding: const EdgeInsets.all(16.0),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              const Text(
                'Question Palette',
                style: TextStyle(
                  fontSize: 18,
                  fontWeight: FontWeight.bold,
                ),
              ),
              IconButton(
                icon: const Icon(Icons.close),
                onPressed: () {
                  setState(() {
                    _showQuestionPalette = false;
                  });
                },
              ),
            ],
          ),
          const SizedBox(height: 16),
          Row(
            children: [
              _buildLegend(Colors.green, 'Answered'),
              const SizedBox(width: 16),
              _buildLegend(Colors.orange, 'Marked'),
              const SizedBox(width: 16),
              _buildLegend(Colors.grey[300]!, 'Not Answered'),
            ],
          ),
          const SizedBox(height: 16),
          Expanded(
            child: GridView.builder(
              gridDelegate: const SliverGridDelegateWithFixedCrossAxisCount(
                crossAxisCount: 5,
                crossAxisSpacing: 8,
                mainAxisSpacing: 8,
              ),
              itemCount: questions.length,
              itemBuilder: (context, index) {
                final question = questions[index];
                final isAnswered = _answers.containsKey(question.questionId);
                final isMarked = _markedForReview.contains(index);
                final isCurrent = index == _currentQuestionIndex;

                Color backgroundColor;
                if (isCurrent) {
                  backgroundColor = Colors.blue;
                } else if (isAnswered) {
                  backgroundColor = Colors.green;
                } else if (isMarked) {
                  backgroundColor = Colors.orange;
                } else {
                  backgroundColor = Colors.grey[300]!;
                }

                return InkWell(
                  onTap: () => _goToQuestion(index),
                  child: Container(
                    decoration: BoxDecoration(
                      color: backgroundColor,
                      borderRadius: BorderRadius.circular(8),
                    ),
                    child: Center(
                      child: Text(
                        '${index + 1}',
                        style: TextStyle(
                          color: isCurrent || isAnswered || isMarked
                              ? Colors.white
                              : Colors.black,
                          fontWeight: FontWeight.bold,
                        ),
                      ),
                    ),
                  ),
                );
              },
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildLegend(Color color, String label) {
    return Row(
      mainAxisSize: MainAxisSize.min,
      children: [
        Container(
          width: 16,
          height: 16,
          decoration: BoxDecoration(
            color: color,
            borderRadius: BorderRadius.circular(4),
          ),
        ),
        const SizedBox(width: 4),
        Text(
          label,
          style: const TextStyle(fontSize: 12),
        ),
      ],
    );
  }

  Color _getDifficultyColor(String difficulty) {
    switch (difficulty.toLowerCase()) {
      case 'easy':
        return Colors.green.withOpacity(0.2);
      case 'intermediate':
        return Colors.orange.withOpacity(0.2);
      case 'hard':
        return Colors.red.withOpacity(0.2);
      default:
        return Colors.grey.withOpacity(0.2);
    }
  }
}
