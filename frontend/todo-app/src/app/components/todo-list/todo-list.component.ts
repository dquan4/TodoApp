import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { Todo } from '../../models/todo.model';
import { TodoService } from '../../services/todo.service';

@Component({
  selector: 'app-todo-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './todo-list.component.html',
})
export class TodoListComponent implements OnInit {
  todos: Todo[] = [];
  newTitle = '';
  loading = false;
  errorMessage = '';

  constructor(private todoService: TodoService) {}

  ngOnInit(): void {
    this.loadTodos();
  }

  loadTodos(): void {
    this.loading = true;
    this.todoService.getAll().subscribe({
      next: (todos) => {
        this.todos = todos;
        this.loading = false;
      },
      error: () => {
        this.errorMessage = 'Could not reach the API. Is the backend running on http://localhost:5000?';
        this.loading = false;
      },
    });
  }

  addTodo(form: NgForm): void {
    const title = (this.newTitle ?? '').trim();
    if (!title) {
      return;
    }

    this.todoService.create(title).subscribe({
      next: (todo) => {
        this.todos = [todo, ...this.todos];
        this.errorMessage = '';
        form.resetForm({ title: '' });
      },
      error: () => (this.errorMessage = 'Failed to add task.'),
    });
  }

  toggleComplete(todo: Todo): void {
    this.todoService.toggle(todo.id).subscribe({
      next: (updated) => {
        todo.isComplete = updated.isComplete;
      },
      error: () => (this.errorMessage = 'Failed to update task.'),
    });
  }

  deleteTodo(todo: Todo): void {
    this.todoService.delete(todo.id).subscribe({
      next: () => {
        this.todos = this.todos.filter((t) => t.id !== todo.id);
      },
      error: () => (this.errorMessage = 'Failed to delete task.'),
    });
  }

  get completedCount(): number {
    return this.todos.filter((t) => t.isComplete).length;
  }
}
