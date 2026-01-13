import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Sprint, CreateSprint, UpdateSprint } from '../models/sprint.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SprintService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getByProjectId(projectId: number): Observable<Sprint[]> {
    return this.http.get<Sprint[]>(`${this.apiUrl}/projects/${projectId}/sprints`);
  }

  getById(id: number): Observable<Sprint> {
    return this.http.get<Sprint>(`${this.apiUrl}/sprints/${id}`);
  }

  create(projectId: number, sprint: CreateSprint): Observable<Sprint> {
    return this.http.post<Sprint>(`${this.apiUrl}/projects/${projectId}/sprints`, sprint);
  }

  update(id: number, sprint: UpdateSprint): Observable<Sprint> {
    return this.http.put<Sprint>(`${this.apiUrl}/sprints/${id}`, sprint);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/sprints/${id}`);
  }
}
