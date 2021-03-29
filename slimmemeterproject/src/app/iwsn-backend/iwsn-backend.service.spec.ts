import { TestBed } from '@angular/core/testing';

import { IwsnBackendService } from './iwsn-backend.service';

describe('IwsnBackendService', () => {
  let service: IwsnBackendService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(IwsnBackendService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
