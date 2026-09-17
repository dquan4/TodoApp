import { TestBed } from '@angular/core/testing';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import { provideHttpClient } from '@angular/common/http';
import { TodoService } from './todo.service';
import { Todo } from '../models/todo.model';

describe('TodoService', () => {
  let service: TodoService;
  let http: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [TodoService, provideHttpClient(), provideHttpClientTesting()],
    });
    service = TestBed.inject(TodoService);
    http = TestBed.inject(HttpTestingController);
  });

  afterEach(() => http.verify());

  it('gets all todos from the API', () => {
    const todos: Todo[] = [
      { id: 1, title: 'Task', isComplete: false, createdAt: '2026-01-01' },
    ];
    let result: Todo[] | undefined;

    service.getAll().subscribe((value) => (result = value));

    const request = http.expectOne('http://127.0.0.1:5000/api/todos');
    expect(request.request.method).toBe('GET');
    request.flush(todos);
    expect(result).toEqual(todos);
  });

  it('creates a todo with the supplied title', () => {
    const todo: Todo = {
      id: 3,
      title: 'New task',
      isComplete: false,
      createdAt: '2026-01-01',
    };

    service.create('New task').subscribe();

    const request = http.expectOne('http://127.0.0.1:5000/api/todos');
    expect(request.request.method).toBe('POST');
    expect(request.request.body).toEqual({ title: 'New task' });
    request.flush(todo);
  });

  it('toggles and deletes the requested todo', () => {
    service.toggle(2).subscribe();
    const toggleRequest = http.expectOne('http://127.0.0.1:5000/api/todos/2/toggle');
    expect(toggleRequest.request.method).toBe('PATCH');
    expect(toggleRequest.request.body).toEqual({});
    toggleRequest.flush({});

    service.delete(2).subscribe();
    const deleteRequest = http.expectOne('http://127.0.0.1:5000/api/todos/2');
    expect(deleteRequest.request.method).toBe('DELETE');
    deleteRequest.flush(null);
  });
});
