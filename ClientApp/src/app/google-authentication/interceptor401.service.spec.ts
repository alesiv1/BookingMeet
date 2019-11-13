/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { Interceptor401Service } from './interceptor401.service';

describe('Service: Interceptor401', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [Interceptor401Service]
    });
  });

  it('should ...', inject([Interceptor401Service], (service: Interceptor401Service) => {
    expect(service).toBeTruthy();
  }));
});
