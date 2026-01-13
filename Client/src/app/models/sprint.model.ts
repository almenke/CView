export interface Sprint {
  id: number;
  projectId: number;
  name: string;
  startsAt: Date;
  endsAt: Date;
  createdAt: Date;
  updatedAt: Date;
}

export interface CreateSprint {
  name: string;
  startsAt: Date;
  endsAt: Date;
}

export interface UpdateSprint {
  name: string;
  startsAt: Date;
  endsAt: Date;
}
