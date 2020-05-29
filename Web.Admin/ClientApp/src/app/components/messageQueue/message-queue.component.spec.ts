import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MessageQueueComponent } from './message-queue.component';

describe('MessageQueueComponent', () => {
  let component: MessageQueueComponent;
  let fixture: ComponentFixture<MessageQueueComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MessageQueueComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MessageQueueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
