import { Permission } from "./permission.type";

export type RolePermissionsUpdateResponse = {
    roleId: string;
    permissionIds: Array<Permission>;
}