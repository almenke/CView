import { Sprint } from './sprint.model';
import { Owner } from './owner.model';
import { Task } from './task.model';

export interface Project {
  id: number;
  name: string;
  ownerId?: number;
  ownerName?: string;
  startsAt: Date;
  endsAt: Date;
  createdAt: Date;
  updatedAt: Date;
  sprints: Sprint[];
  owners: Owner[];
  tasks: Task[];
}

export interface CreateProject {
  name: string;
  ownerId?: number;
  startsAt: Date;
  endsAt: Date;
}

export interface UpdateProject {
  name: string;
  ownerId?: number;
  startsAt: Date;
  endsAt: Date;
}
