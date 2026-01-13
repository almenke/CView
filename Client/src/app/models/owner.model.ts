export interface Owner {
  id: number;
  projectId: number;
  name: string;
  title: string;
  createdAt: Date;
  updatedAt: Date;
}

export interface CreateOwner {
  name: string;
  title: string;
}

export interface UpdateOwner {
  name: string;
  title: string;
}
