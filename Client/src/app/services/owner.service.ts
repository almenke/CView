import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Owner, CreateOwner, UpdateOwner } from '../models/owner.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class OwnerService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getByProjectId(projectId: number): Observable<Owner[]> {
    return this.http.get<Owner[]>(`${this.apiUrl}/projects/${projectId}/owners`);
  }

  getById(id: number): Observable<Owner> {
    return this.http.get<Owner>(`${this.apiUrl}/owners/${id}`);
  }

  create(projectId: number, owner: CreateOwner): Observable<Owner> {
    return this.http.post<Owner>(`${this.apiUrl}/projects/${projectId}/owners`, owner);
  }

  update(id: number, owner: UpdateOwner): Observable<Owner> {
    return this.http.put<Owner>(`${this.apiUrl}/owners/${id}`, owner);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/owners/${id}`);
  }
}
