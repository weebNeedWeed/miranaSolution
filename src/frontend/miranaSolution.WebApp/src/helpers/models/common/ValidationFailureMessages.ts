export type ValidationFailureMessages<T> = {
    [P in keyof T]: string;
};