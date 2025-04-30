export type User = {
    id: string;
    firstName: string;
    lastName: string;
    username: string;
    email: string;
    dateCreated: string;
    role: {
        id: string;
        roleName: string;
    };
    permissions: Array<{
        id: string;
        permissionName: string;
    }>;
}