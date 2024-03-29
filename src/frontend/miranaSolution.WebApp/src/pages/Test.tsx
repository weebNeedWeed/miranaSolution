import React, {useState} from "react";
import {Rating} from "../components";

const Test = (): JSX.Element => {
    const [star, setStar] = useState<number>(0);

    return (
        <div className="p-32">
            <Rating value={star}
                    readonly={false}
                    onChange={setStar}/>

        </div>
    );
};

export {Test};
