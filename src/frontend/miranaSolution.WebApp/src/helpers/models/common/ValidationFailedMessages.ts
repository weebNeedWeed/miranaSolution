export type ValidationFailedMessages<T> = {
    [P in keyof T]: string[];
};