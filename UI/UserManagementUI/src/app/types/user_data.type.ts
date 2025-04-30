import { User } from "./user.type";

export type UsersData = {
    users: Array<User>;
    pageNumber: number;
    pageSize: number;
    totalCount: number;
};