export enum StatusEnum {
  NotSet = 0,
  Backlog = 1,
  Active = 2,
  Complete = 3
}

export interface Task {
  id: number;
  projectId: number;
  name: string;
  ownerId?: number;
  ownerName?: string;
  plannedStartsAt: Date;
  plannedEndsAt: Date;
  actualStartsAt?: Date;
  actualEndsAt?: Date;
  status: StatusEnum;
  createdAt: Date;
  updatedAt: Date;
}

export interface CreateTask {
  name: string;
  ownerId?: number;
  plannedStartsAt: Date;
  plannedEndsAt: Date;
  actualStartsAt?: Date;
  actualEndsAt?: Date;
  status: StatusEnum;
}

export interface UpdateTask {
  name: string;
  ownerId?: number;
  plannedStartsAt: Date;
  plannedEndsAt: Date;
  actualStartsAt?: Date;
  actualEndsAt?: Date;
  status: StatusEnum;
}
