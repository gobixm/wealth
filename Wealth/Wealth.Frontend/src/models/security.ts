export interface Security {
  id: string;
  name: string;
  modified: Date;
  deleted: boolean;
  term: number;
}

export interface SecuritySyncProgress {
  id: string;
  completed: boolean;
  count: number;
}
