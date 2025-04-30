export type RolePermissions = {
    roleId: string;
    roleName: string;
    permissions: Array<{
        id: string;
        permissionName: string;
        isSelected: boolean;
    }>;
}