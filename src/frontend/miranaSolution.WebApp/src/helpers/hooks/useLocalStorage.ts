import {useEffect, useState} from "react";

export function useLocalStorage<TData>(key: string, defaultValue: TData) {
    const [value, setValue] = useState<TData>(() => {
        const data = localStorage.getItem(key);
        if (!data) {
            return defaultValue;
        }

        return JSON.parse(data) as TData;
    });

    useEffect(() => {
        
    }, [setValue]);

    useEffect(() => {
        const stringified = JSON.stringify(value);
        localStorage.setItem(key, stringified);
    }, [value, defaultValue]);

    return [value, setValue] as [typeof value, typeof setValue];
}
