import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NgForm } from '@angular/forms';
import { of, throwError } from 'rxjs';
import { TodoListComponent } from './todo-list.component';
import { TodoService } from '../../services/todo.service';
import { Todo } from '../../models/todo.model';

const todos: Todo[] = [
  { id: 1, title: 'Open task', isComplete: false, createdAt: '2026-01-01' },
  { id: 2, title: 'Done task', isComplete: true, createdAt: '2026-01-02' },
];

describe('TodoListComponent', () => {
  let fixture: ComponentFixture<TodoListComponent>;
  let component: TodoListComponent;
  let todoService: jasmine.SpyObj<TodoService>;

  beforeEach(async () => {
    todoService = jasmine.createSpyObj('TodoService', [
      'getAll',
      'create',
      'toggle',
      'delete',
    ]);
    todoService.getAll.and.returnValue(of(todos));

    await TestBed.configureTestingModule({
      imports: [TodoListComponent],
      providers: [{ provide: TodoService, useValue: todoService }],
    }).compileComponents();

    fixture = TestBed.createComponent(TodoListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('loads todos and counts completed items', () => {
    expect(todoService.getAll).toHaveBeenCalled();
    expect(component.todos).toEqual(todos);
    expect(component.completedCount).toBe(1);
    expect(component.loading).toBeFalse();
  });

  it('adds a trimmed todo and resets the form', () => {
    const created: Todo = {
      id: 3,
      title: 'New task',
      isComplete: false,
      createdAt: '2026-01-03',
    };
    todoService.create.and.returnValue(of(created));
    component.newTitle = '  New task  ';
    const form = {
      resetForm: jasmine.createSpy('resetForm'),
    } as unknown as NgForm;

    component.addTodo(form);

    expect(todoService.create).toHaveBeenCalledWith('New task');
    expect(component.todos[0]).toEqual(created);
    expect(form.resetForm).toHaveBeenCalledWith({ title: '' });
  });

  it('updates completion and removes a deleted todo', () => {
    todoService.toggle.and.returnValue(of({ ...todos[0], isComplete: true }));
    todoService.delete.and.returnValue(of(void 0));

    component.toggleComplete(component.todos[0]);
    component.deleteTodo(component.todos[1]);

    expect(component.todos[0].isComplete).toBeTrue();
    expect(component.todos).toEqual([todos[0]]);
  });

  it('reports loading failures', () => {
    todoService.getAll.and.returnValue(throwError(() => new Error('offline')));

    component.loadTodos();

    expect(component.loading).toBeFalse();
    expect(component.errorMessage).toContain('Could not reach the API');
  });
});
