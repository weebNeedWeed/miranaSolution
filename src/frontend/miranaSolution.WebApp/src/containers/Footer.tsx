import {Link} from "react-router-dom";
import logo from "../assets/logo.svg";
import {BsFacebook, BsTwitter, BsReddit, BsTelegram} from "react-icons/bs";
import {Section} from "../components/Section";

const Footer = (): JSX.Element => {
    return (
        <div className="py-20 sm:py-40 bg-gradient bg-oldRose mt-4">
            <Section className="py-0">
                <div className="flex flex-col sm:flex-row w-full gap-y-4 gap-2 justify-between w-full">
                    <div className="flex flex-col">
                        <Link to={"/"}>
                            <img src={logo}/>
                            <p className="text-sm text-deepKoamaru">&copy; 2023</p>
                        </Link>
                    </div>

                    <div className="flex flex-col">
                        <p className="font-bold text-deepKoamaru text-base">Tài khoản</p>
                        <Link to={"/"} className="text-deepKoamaru text-bas">
                            Đăng ký
                        </Link>
                        <Link to={"/"} className="text-deepKoamaru text-bas">
                            Đăng nhập
                        </Link>
                    </div>

                    <div className="flex flex-col">
                        <p className="font-bold text-deepKoamaru text-base">Truyện</p>
                        <Link to={"/"} className="text-deepKoamaru text-bas">
                            Tìm kiếm
                        </Link>
                        <Link to={"/"} className="text-deepKoamaru text-bas">
                            Đọc truyện
                        </Link>
                    </div>

                    <div className="flex flex-col">
                        <p className="font-bold text-deepKoamaru text-base">Bảo mật</p>
                        <Link to={"/"} className="text-deepKoamaru text-bas">
                            Chính sách
                        </Link>
                        <Link to={"/"} className="text-deepKoamaru text-bas">
                            Điều khoản
                        </Link>
                    </div>

                    <div className="flex flex-col">
                        <p className="font-bold text-deepKoamaru text-base">
                            Theo dõi chúng tôi qua
                        </p>
                        <div className="flex flex-row gap-x-2 text-2xl text-deepKoamaru mt-1">
                            <Link to={"/"}>
                                <BsFacebook/>
                            </Link>
                            <Link to={"/"}>
                                <BsTwitter/>
                            </Link>
                            <Link to={"/"}>
                                <BsReddit/>
                            </Link>
                            <Link to={"/"}>
                                <BsTelegram/>
                            </Link>
                        </div>
                    </div>
                </div>
            </Section>
        </div>
    );
};

export {Footer};
